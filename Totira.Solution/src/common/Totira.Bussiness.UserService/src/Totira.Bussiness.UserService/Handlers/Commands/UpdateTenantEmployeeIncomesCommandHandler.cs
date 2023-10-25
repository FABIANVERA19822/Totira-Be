using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Extensions.BuisinessExtensions.Profile;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.EventServiceBus;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantEmployeeIncomesCommandHandler
        : IMessageHandler<UpdateTenantEmployeeIncomesCommand>
    {
        private readonly ILogger<UpdateTenantEmployeeIncomesCommandHandler> _logger;
        private readonly IRepository<TenantEmployeeIncomes, Guid> _repository;
        private readonly IRepository<TenantApplicationDetails, Guid> _repositoryApplicationDetails;
        private readonly IEventBus _bus;
        private readonly IContextFactory _contextFactory;
        private readonly IS3Handler _s3Handler;

        public UpdateTenantEmployeeIncomesCommandHandler(
            ILogger<UpdateTenantEmployeeIncomesCommandHandler> logger,
            IRepository<TenantEmployeeIncomes, Guid> repository,
            IRepository<TenantApplicationDetails, Guid> repositoryApplicationDetails,
            IS3Handler s3Handler,
            IEventBus bus,
            IContextFactory contextFactory
            )
        {
            _logger = logger;
            _repository = repository;
            _repositoryApplicationDetails = repositoryApplicationDetails;
            _s3Handler = s3Handler;
            _bus = bus;
            _contextFactory = contextFactory;
        }

        public async Task HandleAsync(IContext context, UpdateTenantEmployeeIncomesCommand command)
        {
            var tenant = await _repository.GetByIdAsync(command.TenantId);
            _logger.LogInformation("Tenant ID: {tenantId}", command.TenantId);

            if (tenant is null || tenant.EmployeeIncomes is null)
                return;

            tenant.IsStudent = (tenant.EmployeeIncomes.Any());

            var income = tenant
                .EmployeeIncomes
                .FirstOrDefault(income => income.Id == command.IncomeId);

            if (income is null)
                return;

            _logger.LogInformation("Income ID: {incomeId}", income.Id);

            // Removing old files
            if (income.Files is not null && income.Files.Any())
            {
                _logger.LogInformation("Starts to iterate files to be deleted.");
                foreach (var fileName in command.DeletedFiles)
                {
                    var fileInfo = income
                        .Files
                        .SingleOrDefault(fileInfo => fileInfo.FileName == fileName);

                    if (fileInfo is null) continue;

                    var isDeleted = await _s3Handler.DeleteObjectAsync(fileInfo.S3KeyName);

                    if (isDeleted)
                    {
                        income.Files.Remove(fileInfo);

                        _logger.LogInformation("File: [name: {fileName}, key: {key}] has beend deleted.", fileInfo.FileName, fileInfo.S3KeyName);
                    }
                }
            }

            // Adding some new files
            _logger.LogInformation("Starts to iterate files to be uploaded.");
            foreach (var newFile in command.NewFiles)
            {
                var key = GetFormattedKeyName(
                    tenantId: command.TenantId.ToString(),
                    incomeId: income.Id.ToString(),
                    fileName: newFile.FileName);

                var isUploaded = await UploadFileToS3Async(key, newFile.ContentType, newFile.Data!);

                if (isUploaded)
                {
                    income.AddNewFileInfo(newFile.FileName, key, newFile.ContentType, newFile.Size);

                    _logger.LogInformation("File: [name: {fileName}, key: {key}] added to file info list.", newFile.FileName, key);
                }
            }

            income.UpdateInformation(
                command.CompanyOrganizationName,
                command.Position,
                command.StartDate!.Value,
                command.IsCurrentlyWorking,
                command.IsCurrentlyWorking ? null : command.EndDate!.Value,
                command.IsCurrentlyWorking ? command.MonthlyIncome!.Value : null,
                command.IncomeId);

            income.ContactReference.UpdateInformation(
                command.ContactReference.FirstName,
                command.ContactReference.LastName,
                command.ContactReference.JobTitle,
                command.ContactReference.Relationship,
                command.ContactReference.Email);

            income.ContactReference.PhoneNumber.UpdateInformation(
                command.ContactReference.PhoneNumber.Value,
                command.ContactReference.PhoneNumber.CountryCode);

            await _repository.Update(tenant);

            _logger.LogInformation("Tenant employee income data updated.");
            await _repositoryApplicationDetails.VerifyAndUpdateJiraAndVerifiedProfile(_contextFactory, _bus, command.TenantId);
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
