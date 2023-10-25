﻿using LanguageExt;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.Events;
using Totira.Bussiness.UserService.Extensions.BuisinessExtensions.Profile;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.EventServiceBus;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantEmployeeIncomesCommandHandler : IMessageHandler<CreateTenantEmployeeIncomesCommand>
    {
        private readonly ILogger<CreateTenantEmployeeIncomesCommandHandler> _logger;
        private readonly IRepository<TenantEmployeeIncomes, Guid> _repository;
        private readonly IRepository<TenantVerificationProfile, Guid> _repositoryVerificationProfile;
        private readonly IRepository<TenantCurrentJobStatus, Guid> _tenantCurrentJobStatusRepository;
        private readonly IEventBus _bus;
        private readonly IContextFactory _contextFactory;
        private readonly IS3Handler _s3Handler;
        private readonly IMessageService _messageService;

        public CreateTenantEmployeeIncomesCommandHandler(
            IRepository<TenantEmployeeIncomes, Guid> repository,
            ILogger<CreateTenantEmployeeIncomesCommandHandler> logger,
            IRepository<TenantVerificationProfile, Guid> repositoryVerificationProfile,
            IRepository<TenantCurrentJobStatus, Guid> tenantCurrentJobStatusRepository,
            IS3Handler s3Handler,
            IEventBus bus,
            IContextFactory contextFactory,
            IMessageService messageService)
        {
            _repository = repository;
            _logger = logger;
            _repositoryVerificationProfile = repositoryVerificationProfile;
            _tenantCurrentJobStatusRepository = tenantCurrentJobStatusRepository;
            _s3Handler = s3Handler;
            _bus = bus;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, Either<Exception, CreateTenantEmployeeIncomesCommand> command)
        {
            await command.MatchAsync(async cmd => {
                Guid? messageOutboxId = null;
                var tenantIncomes = await _repository.GetByIdAsync(cmd.TenantId);

                if (tenantIncomes is null)
                {
                    tenantIncomes = TenantEmployeeIncomes.Create(cmd.TenantId);
                    await _repository.Add(tenantIncomes);
                }

                tenantIncomes.IsStudent = false;

                _logger.LogInformation("Created tenant employee incomes for tenant: {TenantId}", cmd.TenantId);

                var newIdEmployeeIncome = Guid.NewGuid();
                _logger.LogInformation("New employee income id: {IncomeId}", newIdEmployeeIncome);

            var fileInfos = new List<Domain.Common.File>();

                _logger.LogInformation("Starts to iterate files to be uploaded.");

                foreach (var file in cmd.Files)
                {
                    var key = GetFormattedKeyName(
                        tenantId: cmd.TenantId.ToString(),
                        incomeId: newIdEmployeeIncome.ToString(),
                        fileName: file.FileName);

                    var isUploaded = await UploadFileToS3Async(key, file.ContentType, file.Data!);

                if (isUploaded)
                {
                    var fileInfo = Domain.Common.File.Create(file.FileName, key, file.ContentType, file.Size);
                    fileInfos.Add(fileInfo);
                    _logger.LogInformation("File: [name: {fileName}, key: {key}] added to file info list.", file.FileName, key);
                }
            }

                var contactReference = Domain.EmploymentContactReference.Create(
                firstName: cmd.ContactReference.FirstName,
                lastName: cmd.ContactReference.LastName,
                jobTitle: cmd.ContactReference.JobTitle,
                relationship: cmd.ContactReference.Relationship,
                email: cmd.ContactReference.Email,
                phoneNumber: Domain.EmploymentContactReferencePhoneNumber.Create(
                    value: cmd.ContactReference.PhoneNumber.Value,
                    countryCode: cmd.ContactReference.PhoneNumber.CountryCode));

                tenantIncomes.EmployeeIncomes ??= new();
                tenantIncomes.EmployeeIncomes.Add(TenantEmployeeIncome.Create(
                    id: newIdEmployeeIncome,
                    tenantId: cmd.TenantId,
                    companyOrganizationName: cmd.CompanyOrganizationName,
                    position: cmd.Position,
                    startDate: cmd.StartDate!.Value,
                    isCurrentlyWorking: cmd.IsCurrentlyWorking,
                    endDate: cmd.IsCurrentlyWorking ? null : cmd.EndDate,
                    monthlyIncome: cmd.IsCurrentlyWorking ? cmd.MonthlyIncome : null,
                    contactReference: contactReference,
                    files: fileInfos));

                await _repository.Update(tenantIncomes);
                _logger.LogInformation("Tenant employee income added.");

            if (cmd.IsCurrentlyWorking)
            {
                // await _repositoryApplicationDetails.VerifyAndUpdateJiraAndVerifiedProfile(_contextFactory, _bus, cmd.TenantId); 
                await _repositoryVerificationProfile.UnverifyTenant(cmd.TenantId, _contextFactory, _bus);
            }

                // Create TenantCurrentJobStatus
                Expression<Func<TenantCurrentJobStatus, bool>> expression = p => p.TenantId == cmd.TenantId;

                var actualData = (await _tenantCurrentJobStatusRepository.Get(expression)).FirstOrDefault();

                if (actualData == null)
                {
                    var tenantCurrentJobStatus = TenantCurrentJobStatus.CreateCurrentJobStatus(
                        cmd.TenantId,
                        "",
                        false);
                    await _tenantCurrentJobStatusRepository.Add(tenantCurrentJobStatus);
                }

                // Create Event
                var userCreatedEvent = new TenantEmployeeIncomesCreatedEvent(tenantIncomes.Id, "Employee Created");
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
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
