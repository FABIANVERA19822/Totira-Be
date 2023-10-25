using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Bussiness.UserService.Extensions.BuisinessExtensions.Profile;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class CreateTenantStudentFinancialDetailCommandHandler : IMessageHandler<CreateTenantStudentFinancialDetailCommand>
{
    private readonly ILogger<CreateTenantStudentFinancialDetailCommandHandler> _logger;
    private readonly IRepository<TenantEmployeeIncomes, Guid> _incomesRepository;
    private readonly IRepository<TenantApplicationDetails, Guid> _repositoryApplicationDetails;
    private readonly IEventBus _bus;
    private readonly IContextFactory _contextFactory;
    private readonly ICommonFunctions _helper;
    private readonly IMessageService _messageService;

    public CreateTenantStudentFinancialDetailCommandHandler(
        ILogger<CreateTenantStudentFinancialDetailCommandHandler> logger,
        IRepository<TenantEmployeeIncomes, Guid> incomesRepository,
        IRepository<TenantApplicationDetails, Guid> repositoryApplicationDetails,
        ICommonFunctions helper,
        IEventBus bus,
        IContextFactory contextFactory,
        IMessageService messageService)
    {
        _logger = logger;
        _incomesRepository = incomesRepository;
        _repositoryApplicationDetails = repositoryApplicationDetails;
        _helper = helper;
        _bus = bus;
        _contextFactory = contextFactory;
        _messageService = messageService;
    }

    public async Task HandleAsync(IContext context, CreateTenantStudentFinancialDetailCommand command)
    {
        Guid? messageOutboxId = null;
        _logger.LogInformation("New tenant student financial detail for tenant {tenantId}", command.TenantId);

        var tenantIncomes = await _incomesRepository.GetByIdAsync(command.TenantId);

        if (tenantIncomes is null)
        {
            tenantIncomes = TenantEmployeeIncomes.Create(command.TenantId);
            await _incomesRepository.Add(tenantIncomes);
        }

        tenantIncomes.IsStudent = tenantIncomes.EmployeeIncomes is null || !tenantIncomes.EmployeeIncomes.Any();

        var studentFinancialDetail = TenantStudentFinancialDetail.Create(
            command.UniversityOrInstitute,
            command.Degree,
            command.IsOverseasStudent);

        _logger.LogInformation("Study detail created: {studyId}", studentFinancialDetail.Id);

        var keyName = GetS3KeyNamePreffix(tenantIncomes.Id, studentFinancialDetail.Id);

        // Enrollment proofs
        foreach (var file in command.EnrollmentProofs)
        {
            var enrollmentProof = await _helper.UploadFileAsync(keyName, file);
            if (enrollmentProof is not null)
            {
                studentFinancialDetail.EnrollmentProofs.Add(enrollmentProof);
                _logger.LogInformation("New proof of enrollment saved: {fileName}", enrollmentProof.FileName);
            }
        }

        // Study permits or visas
        if (command.IsOverseasStudent && command.StudyPermitsOrVisas is not null)
            foreach (var file in command.StudyPermitsOrVisas)
            {
                var studyPermitOrVisa = await _helper.UploadFileAsync(keyName, file);
                if (studyPermitOrVisa is not null)
                {
                    studentFinancialDetail.StudyPermitsOrVisas.Add(studyPermitOrVisa);
                    _logger.LogInformation("New study permit or visa saved: {fileName}", studyPermitOrVisa.FileName);
                }
            }

        // Income proofs
        foreach (var file in command.IncomeProofs)
        {
            var incomeProof = await _helper.UploadFileAsync(keyName, file);
            if (incomeProof is not null)
            {
                studentFinancialDetail.IncomeProofs.Add(incomeProof);
                _logger.LogInformation("New proof of income saved: {fileName}", incomeProof.FileName);
            }
        }

        tenantIncomes.StudentIncomes ??= new();
        tenantIncomes.StudentIncomes.Add(studentFinancialDetail);

        _logger.LogInformation("Study detail added to tenant {tenantId}", tenantIncomes.Id);

        await _incomesRepository.Update(tenantIncomes);

        _logger.LogInformation("Study details: Changes saved!");

        await _repositoryApplicationDetails.VerifyAndUpdateJiraAndVerifiedProfile(_contextFactory, _bus, command.TenantId);

        var studentDetailCreatedEvent = new TenantStudentFinancialDetailCreatedEvent(command.TenantId, "Study Created");
        var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
        messageOutboxId = await _messageService.SendAsync(notificationContext, studentDetailCreatedEvent);
    }

    private static string GetS3KeyNamePreffix(Guid tenantId, Guid studyId)
        => $"{tenantId}/{studyId}";
}
