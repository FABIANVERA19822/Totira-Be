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
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantEmployeeIncomesCommandHandler
        : IMessageHandler<CreateTenantEmployeeIncomesCommand>
    {
        private readonly ILogger<CreateTenantEmployeeIncomesCommandHandler> _logger;
        private readonly IRepository<TenantEmployeeIncomes, Guid> _repository;
        private readonly IRepository<TenantApplicationDetails, Guid> _repositoryApplicationDetails;
        private readonly IRepository<TenantCurrentJobStatus, Guid> _tenantCurrentJobStatusRepository;
        private readonly IEventBus _bus;
        private readonly IContextFactory _contextFactory;
        private readonly IS3Handler _s3Handler;
        private readonly IMessageService _messageService;

        public CreateTenantEmployeeIncomesCommandHandler(
            IRepository<TenantEmployeeIncomes, Guid> repository,
            ILogger<CreateTenantEmployeeIncomesCommandHandler> logger,
            IRepository<TenantApplicationDetails, Guid> repositoryApplicationDetails,
            IRepository<TenantCurrentJobStatus, Guid> tenantCurrentJobStatusRepository,
            IS3Handler s3Handler,
            IEventBus bus,
            IContextFactory contextFactory,
            IMessageService messageService)
        {
            _repository = repository;
            _logger = logger;
            _repositoryApplicationDetails = repositoryApplicationDetails;
            _tenantCurrentJobStatusRepository = tenantCurrentJobStatusRepository;
            _s3Handler = s3Handler;
            _bus = bus;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, CreateTenantEmployeeIncomesCommand command)
        {
            Guid? messageOutboxId = null;
            var tenantIncomes = await _repository.GetByIdAsync(command.TenantId);

            if (tenantIncomes is null)
            {
                tenantIncomes = TenantEmployeeIncomes.Create(command.TenantId);
                await _repository.Add(tenantIncomes);
            }

            tenantIncomes.IsStudent = false;

            _logger.LogInformation("Created tenant employee incomes for tenant: {tenantId}", command.TenantId);

            var newIdEmployeeIncome = Guid.NewGuid();
            _logger.LogInformation("New employee income id: {incomeId}", newIdEmployeeIncome);

            var fileInfos = new List<TenantFile>();

            _logger.LogInformation("Starts to iterate files to be uploaded.");

            foreach (var file in command.Files)
            {
                var key = GetFormattedKeyName(
                    tenantId: command.TenantId.ToString(),
                    incomeId: newIdEmployeeIncome.ToString(),
                    fileName: file.FileName);

                var isUploaded = await UploadFileToS3Async(key, file.ContentType, file.Data!);

                if (isUploaded)
                {
                    var fileInfo = TenantFile.Create(file.FileName, key, file.ContentType, file.Size);
                    fileInfos.Add(fileInfo);
                    _logger.LogInformation("File: [name: {fileName}, key: {key}] added to file info list.", file.FileName, key);
                }
            }

            var contactReference = Domain.EmploymentContactReference.Create(
            firstName: command.ContactReference.FirstName,
            lastName: command.ContactReference.LastName,
            jobTitle: command.ContactReference.JobTitle,
            relationship: command.ContactReference.Relationship,
            email: command.ContactReference.Email,
            phoneNumber: Domain.EmploymentContactReferencePhoneNumber.Create(
                value: command.ContactReference.PhoneNumber.Value,
                countryCode: command.ContactReference.PhoneNumber.CountryCode));

            tenantIncomes.EmployeeIncomes ??= new();
            tenantIncomes.EmployeeIncomes.Add(TenantEmployeeIncome.Create(
                id: newIdEmployeeIncome,
                tenantId: command.TenantId,
                companyOrganizationName: command.CompanyOrganizationName,
                position: command.Position,
                startDate: command.StartDate!.Value,
                isCurrentlyWorking: command.IsCurrentlyWorking,
                endDate: command.IsCurrentlyWorking ? null : command.EndDate,
                monthlyIncome: command.IsCurrentlyWorking ? command.MonthlyIncome : null,
                contactReference: contactReference,
                files: fileInfos));

            await _repository.Update(tenantIncomes);
            _logger.LogInformation("Tenant employee income added.");

            if (command.IsCurrentlyWorking)
            {
                await _repositoryApplicationDetails.VerifyAndUpdateJiraAndVerifiedProfile(_contextFactory, _bus, command.TenantId); 
            }

            // Create TenantCurrentJobStatus
            Expression<Func<TenantCurrentJobStatus, bool>> expression = (p => p.TenantId == command.TenantId);

            var actualData = (await _tenantCurrentJobStatusRepository.Get(expression)).FirstOrDefault();

            if (actualData == null)
            {
                var tenantCurrentJobStatus = TenantCurrentJobStatus.CreateCurrentJobStatus(
                    command.TenantId,
                    "",
                    false);
                await _tenantCurrentJobStatusRepository.Add(tenantCurrentJobStatus);
            }

            // Create Event
            var userCreatedEvent = new TenantEmployeeIncomesCreatedEvent(tenantIncomes.Id, "Employee Created");
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
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
