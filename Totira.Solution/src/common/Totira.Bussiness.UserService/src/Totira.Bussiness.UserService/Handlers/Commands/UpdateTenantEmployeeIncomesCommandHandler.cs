using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO.ThirdpartyService;
using Totira.Bussiness.UserService.Events;
using Totira.Bussiness.UserService.Extensions.BuisinessExtensions.Profile;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.EventServiceBus;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantEmployeeIncomesCommandHandler : IMessageHandler<UpdateTenantEmployeeIncomesCommand>
    {
        public const string Edited = "EDITED";
        public const string Deleted = "DELETED";
        private readonly ILogger<UpdateTenantEmployeeIncomesCommandHandler> _logger;
        private readonly IRepository<TenantEmployeeIncomes, Guid> _repository;
        private readonly IRepository<TenantApplicationDetails, Guid> _repositoryApplicationDetails;
        private readonly IRepository<TenantCurrentJobStatus, Guid> _repositoryCurrentJobStatus;
        private readonly RestClientOptions _restClientOptions;
        private readonly IQueryRestClient _queryRestClient;
        private readonly IEventBus _bus;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        private readonly IS3Handler _s3Handler;

        public UpdateTenantEmployeeIncomesCommandHandler(
            ILogger<UpdateTenantEmployeeIncomesCommandHandler> logger,
            IRepository<TenantEmployeeIncomes, Guid> repository,
            IRepository<TenantApplicationDetails, Guid> repositoryApplicationDetails,
            IRepository<TenantCurrentJobStatus, Guid> repositoryCurrentJobStatus,
            IOptions<RestClientOptions> restClientOptions,
            IQueryRestClient queryRestClient,
            IS3Handler s3Handler,
            IEventBus bus,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _logger = logger;
            _repository = repository;
            _repositoryApplicationDetails = repositoryApplicationDetails;
            _repositoryCurrentJobStatus = repositoryCurrentJobStatus;
            _restClientOptions = restClientOptions.Value;
            _queryRestClient = queryRestClient;
            _s3Handler = s3Handler;
            _bus = bus;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, Either<Exception, UpdateTenantEmployeeIncomesCommand> command)
        {
            await command.MatchAsync(async cmd => {
                Guid? messageOutboxId = null;
                var tenant = await _repository.GetByIdAsync(cmd.TenantId);
                _logger.LogInformation("Tenant ID: {TenantId}", cmd.TenantId);

                if (tenant is null || tenant.EmployeeIncomes is null)
                    return;

                tenant.IsStudent = tenant.EmployeeIncomes.Any();

                var income = tenant
                    .EmployeeIncomes
                    .FirstOrDefault(income => income.Id == cmd.IncomeId);

                if (income is null)
                    return;

                _logger.LogInformation("Income ID: {IncomeId}", income.Id);

                // Removing old files
                if (income.Files is not null && income.Files.Any())
                {
                    _logger.LogInformation("Starts to iterate files to be deleted.");
                    foreach (var fileName in cmd.DeletedFiles)
                    {
                        var fileInfo = income
                            .Files
                            .SingleOrDefault(fileInfo => fileInfo.FileName == fileName);

                        if (fileInfo is null) continue;

                        var isDeleted = await _s3Handler.DeleteObjectAsync(fileInfo.S3KeyName);

                        if (isDeleted)
                        {
                            income.Files.Remove(fileInfo);

                            _logger.LogInformation("File: [name: {FileName}, key: {Key}] has beend deleted.", fileInfo.FileName, fileInfo.S3KeyName);
                        }
                    }
                }

                // Adding some new files
                _logger.LogInformation("Starts to iterate files to be uploaded.");
                foreach (var newFile in cmd.NewFiles)
                {
                    var key = GetFormattedKeyName(
                        tenantId: cmd.TenantId.ToString(),
                        incomeId: income.Id.ToString(),
                        fileName: newFile.FileName);

                    var isUploaded = await UploadFileToS3Async(key, newFile.ContentType, newFile.Data!);

                    if (isUploaded)
                    {
                        income.AddNewFileInfo(newFile.FileName, key, newFile.ContentType, newFile.Size);

                        _logger.LogInformation("File: [name: {FileName}, key: {Key}] added to file info list.", newFile.FileName, key);
                    }
                }

                income.UpdateInformation(
                    cmd.CompanyOrganizationName,
                    cmd.Position,
                    cmd.StartDate!.Value,
                    cmd.IsCurrentlyWorking,
                    cmd.IsCurrentlyWorking ? null : cmd.EndDate!.Value,
                    cmd.IsCurrentlyWorking ? cmd.MonthlyIncome!.Value : null,
                    cmd.IncomeId);

                income.ContactReference.UpdateInformation(
                    cmd.ContactReference.FirstName,
                    cmd.ContactReference.LastName,
                    cmd.ContactReference.JobTitle,
                    cmd.ContactReference.Relationship,
                    cmd.ContactReference.Email);

                income.ContactReference.PhoneNumber.UpdateInformation(
                    cmd.ContactReference.PhoneNumber.Value,
                    cmd.ContactReference.PhoneNumber.CountryCode);

                await _repository.Update(tenant);

                _logger.LogInformation("Tenant employee income data updated.");

                if (cmd.IsCurrentlyWorking == true)
                {
                    await _repositoryApplicationDetails.VerifyAndUpdateJiraAndVerifiedProfile(_contextFactory, _bus, cmd.TenantId);
                }

                int currentEmploymentsCount = tenant.EmployeeIncomes?.Count ?? 0;

                // Update Tenant Current Job Status
                Expression<Func<TenantCurrentJobStatus, bool>> expression = p => p.TenantId == cmd.TenantId;
                
                var actualData = (await _repositoryCurrentJobStatus.Get(expression)).FirstOrDefault();

                if (cmd.IsCurrentlyWorking)
                {
                    actualData.CurrentJobStatus = Edited;
                }
                else if (!cmd.IsCurrentlyWorking && currentEmploymentsCount == 1)
                {
                    actualData.CurrentJobStatus = Deleted;
                }
                else if (currentEmploymentsCount == 0)
                {
                    actualData.CurrentJobStatus = Deleted;
                }
                else
                {
                    actualData.CurrentJobStatus = string.Empty;
                }

                actualData.IsUnderRevisionSend = false;
                actualData.UpdatedOn = DateTime.UtcNow;
                await _repositoryCurrentJobStatus.Update(actualData);

                // Update Profile Summary flags
                var tenantId = cmd.TenantId;
                var verification = await _queryRestClient.GetAsync<GetTenantVerifiedProfileDto>($"{_restClientOptions.ThirdPartyIntegration}/VerifiedProfile/profiles/{tenantId}");
                var result1 = verification.Content;

                if (result1 != null && result1.Certn == true && result1.Jira == true && result1.Persona == true)
                {
                    Expression<Func<TenantApplicationDetails, bool>> expressionAD = tad => tad.TenantId == cmd.TenantId;

                    var actualDataAD = (await _repositoryApplicationDetails.Get(expressionAD)).FirstOrDefault();
                    actualDataAD.IsProfileValidationComplete = true;
                    actualData.IsUnderRevisionSend = false;
                    actualDataAD.UpdatedOn = DateTime.UtcNow;
                    if (currentEmploymentsCount == 0)
                        await _repositoryApplicationDetails.Update(actualDataAD);
                }

                // Create Event
                var tenantUpdatedEvent = new TenantEmployeeIncomesUpdatedEvent(income.Id, "Employee Updated");
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);
            }, ex => throw ex);
        }
        private async Task<bool> UploadFileToS3Async(string key, string contentType, byte[] data)
        {
            using var ms = new MemoryStream(data);
            var response = await _s3Handler.UploadSMemorySingleFileAsync(key, contentType, ms);
            return response;
        }

        private static string GetFormattedKeyName(string tenantId, string incomeId, string fileName)
            => $"{tenantId}/{incomeId}/{fileName}";
    }
}
