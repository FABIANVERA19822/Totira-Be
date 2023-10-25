
namespace Totira.Bussiness.UserService.Handlers.Commands
{
    using LanguageExt;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Totira.Bussiness.UserService.Commands;
    using Totira.Bussiness.UserService.Domain;
    using Totira.Bussiness.UserService.Domain.Common;
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

    public class DeleteTenantEmployeeIncomeIdCommandHandler : IMessageHandler<DeleteTenantEmployeeIncomeIdCommand>
    {
        public const string Edited = "EDITED";
        public const string Deleted = "DELETED";
        private readonly IS3Handler _s3Handler;
        private readonly IRepository<TenantEmployeeIncomes, Guid> _incomesTenantEmployeerepository;
        private readonly IRepository<TenantEmploymentReference, Guid> _tenantEmploymentReferenceRepository;
        private readonly IRepository<TenantApplicationDetails, Guid> _repositoryApplicationDetails;
        private readonly IRepository<TenantCurrentJobStatus, Guid> _repositoryCurrentJobStatus;
        private readonly RestClientOptions _restClientOptions;
        private readonly IQueryRestClient _queryRestClient;
        private readonly ILogger<DeleteTenantEmployeeIncomeIdCommandHandler> _logger;
        private readonly IEventBus _bus;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public DeleteTenantEmployeeIncomeIdCommandHandler(
            IRepository<TenantEmployeeIncomes, Guid> repository,
            IRepository<TenantEmploymentReference, Guid> tenantEmploymentReferenceRepository,
            IRepository<TenantApplicationDetails, Guid> repositoryApplicationDetails,
            IRepository<TenantCurrentJobStatus, Guid> repositoryCurrentJobStatus,
            IOptions<RestClientOptions> restClientOptions,
            IQueryRestClient queryRestClient,
            ILogger<DeleteTenantEmployeeIncomeIdCommandHandler> logger,
            IS3Handler s3Handler,
            IEventBus bus,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _incomesTenantEmployeerepository = repository;
            _tenantEmploymentReferenceRepository = tenantEmploymentReferenceRepository;
            _repositoryApplicationDetails = repositoryApplicationDetails;
            _repositoryCurrentJobStatus = repositoryCurrentJobStatus;
            _restClientOptions = restClientOptions.Value;
            _queryRestClient = queryRestClient;
            _logger = logger;
            _s3Handler = s3Handler;
            _bus = bus;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public DeleteTenantEmployeeIncomeIdCommandHandler(IRepository<TenantEmployeeIncomes, Guid> object1, IRepository<TenantEmploymentReference, Guid> object2, ILogger<DeleteTenantEmployeeIncomeIdCommandHandler> object3, IS3Handler object4)
        {
        }

        public async Task HandleAsync(IContext context, Either<Exception, DeleteTenantEmployeeIncomeIdCommand> message)
        {
            await message.MatchAsync(async msg => {
            _logger.LogInformation("Starting to delete employment income {IncomeId} detail of tenant {TenantId}", msg.IncomeId, msg.TenantId);
            await DeleteEmployeeIncomeInformationByIdAsync(context, msg);
            }, ex => throw ex);
        }

        private async Task DeleteEmployeeIncomeInformationByIdAsync(IContext context, DeleteTenantEmployeeIncomeIdCommand command)
        {
            Guid? messageOutboxId = null;
            var tenantIncomes = await _incomesTenantEmployeerepository.GetByIdAsync(command.TenantId);

            if (tenantIncomes is null || tenantIncomes.EmployeeIncomes is null)
            {
                _logger.LogInformation("Employee income information does not exist.");
                return;
            }
            var income = tenantIncomes.EmployeeIncomes.FirstOrDefault(income => income.Id == command.IncomeId);
            
            if (income is null)
                return; 

            await DeleteEmployeeReferencesAsync(command.TenantId);
            await DeleteEmployeeIncomesFilesAsync(income.Files);

            tenantIncomes.EmployeeIncomes.Remove(income);

            if (!tenantIncomes.EmployeeIncomes.Any())
                _logger.LogWarning("Last employment income was deleted.");

            tenantIncomes.IsStudent = IfDoesNotRemainEmploymentIncomesAndHasStudentIncomes(tenantIncomes);

            await _incomesTenantEmployeerepository.Update(tenantIncomes);
            // Update Tenant Current Job Status
            Expression<Func<TenantCurrentJobStatus, bool>> expression = (p => p.TenantId == command.TenantId);

            var actualData = (await _repositoryCurrentJobStatus.Get(expression)).FirstOrDefault();

            var currentlyWorkingCount = tenantIncomes.EmployeeIncomes.Where(x => x.IsCurrentlyWorking).Count();

            actualData.CurrentJobStatus = string.Empty;
            if (currentlyWorkingCount == 0)
            {
                actualData.CurrentJobStatus = Deleted;
            }
            actualData.IsUnderRevisionSend = false;
            actualData.UpdatedOn = DateTime.UtcNow;
            await _repositoryCurrentJobStatus.Update(actualData);

            if (currentlyWorkingCount == 0 
                && tenantIncomes.StudentIncomes is not null
                && tenantIncomes.StudentIncomes.Count() == 0)
            {
                await _repositoryApplicationDetails.VerifyAndUpdateJiraAndVerifiedProfile(_contextFactory, _bus, command.TenantId);
            }

            // Update Profile Summary flags
            var tenantId = command.TenantId;
            var verification = await _queryRestClient.GetAsync<GetTenantVerifiedProfileDto>($"{_restClientOptions.ThirdPartyIntegration}/VerifiedProfile/profiles/{tenantId}");
            var result1 = verification.Content;

            if (result1 != null && result1.Certn == true && result1.Jira == true && result1.Persona == true)
            { 
                Expression<Func<TenantApplicationDetails, bool>> expressionAD = (tad => tad.TenantId == command.TenantId);

                var actualDataAD = (await _repositoryApplicationDetails.Get(expressionAD)).FirstOrDefault();
                actualDataAD.IsProfileValidationComplete = true;
                actualData.IsUnderRevisionSend = false;
                actualDataAD.UpdatedOn = DateTime.UtcNow;
                await _repositoryApplicationDetails.Update(actualDataAD);
            }

            // Create Event
            var tenantDeletedEvent = new TenantEmployeeIncomeIdDeletedEvent(command.TenantId, command.IncomeId, "Employee Deleted");
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            messageOutboxId = await _messageService.SendAsync(notificationContext, tenantDeletedEvent);
        }

        private async Task DeleteEmployeeReferencesAsync(Guid tenantId)
        {
            _logger.LogInformation("Starting to delete employment references of tenant {tenantId}", tenantId);

            var references = await _tenantEmploymentReferenceRepository.GetByIdAsync(tenantId);

            if (references is null)
            {
                _logger.LogInformation("Employee references information does not exist.");
                return;
            }

            references = default!;

            await _tenantEmploymentReferenceRepository.Update(references);
        }

        private async Task DeleteEmployeeIncomesFilesAsync(ICollection<File> files)
        {
            foreach (var file in files)
            {
                var isDeleted = await _s3Handler.DeleteObjectAsync(file.S3KeyName);

                if (!isDeleted)
                    _logger.LogError("File {fileName} delete failed.", file.FileName);
            }
        }

        public static bool IfDoesNotRemainEmploymentIncomesAndHasStudentIncomes(TenantEmployeeIncomes tenantIncomes) =>
            (tenantIncomes.EmployeeIncomes is null || tenantIncomes.EmployeeIncomes.Any()) &&
            (tenantIncomes.StudentIncomes is not null && tenantIncomes.StudentIncomes.Any());
    }
}

