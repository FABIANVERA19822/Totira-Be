using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using Totira.Bussiness.UserService.Extensions.BuisinessExtensions.Profile;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Extensions.BuisinessExtensions.FileUpload;
using Totira.Bussiness.UserService.Events;
using Totira.Support.TransactionalOutbox;

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class UpdateTenantStudentFinancialDetailCommandHandler : IMessageHandler<UpdateTenantStudentFinancialDetailCommand>
{
    private readonly ILogger<UpdateTenantStudentFinancialDetailCommandHandler> _logger;
    private readonly IRepository<TenantEmployeeIncomes, Guid> _incomesRepository;
    private readonly ICommonFunctions _helper;
    private readonly IRepository<TenantApplicationDetails, Guid> _repositoryApplicationDetails;
    private readonly IEventBus _bus;
    private readonly IContextFactory _contextFactory;
    private readonly IMessageService _messageService;

    public UpdateTenantStudentFinancialDetailCommandHandler(
        ILogger<UpdateTenantStudentFinancialDetailCommandHandler> logger,
        IRepository<TenantEmployeeIncomes, Guid> incomesRepository,
        ICommonFunctions helper,
        IEventBus bus,
        IContextFactory contextFactory,
        IRepository<TenantApplicationDetails, Guid> repositoryApplicationDetails,
        IMessageService messageService)
    {
        _logger = logger;
        _incomesRepository = incomesRepository;
        _helper = helper;
        _bus = bus;
        _contextFactory = contextFactory;
        _repositoryApplicationDetails = repositoryApplicationDetails;
        _messageService = messageService;
    }

    public async Task HandleAsync(IContext context, UpdateTenantStudentFinancialDetailCommand command)
    {
        Guid? messageOutboxId = null;
        var tenant = await _incomesRepository.GetByIdAsync(command.TenantId);

        if (tenant is null)
        {
            _logger.LogError("Tenant not found: {tenantId}.", command.TenantId);
            return;
        }
        
        tenant.StudentIncomes ??= new();
        var studyDetail = tenant.StudentIncomes.FirstOrDefault(income => income.Id == command.StudyId);

        if (studyDetail is null)
        {
            _logger.LogError("Study detail not found: {studyId}.", command.StudyId);
            return;
        }
        
        studyDetail.UpdateInformation(
            command.UniversityOrInstitute,
            command.Degree,
            command.IsOverseasStudent);

        _logger.LogInformation("Study detail info updated.");

        // Delete enrollment proof
        if (studyDetail.EnrollmentProofs.Any() && command.DeletedEnrollmentProofs.Any())
        {
            foreach (var name in command.DeletedEnrollmentProofs)
            {
                var file = studyDetail.EnrollmentProofs.FirstOrDefault(file => file.FileName == name);
                if (file is null)
                {
                    _logger.LogError("Proof of enrollment {fileName} not found.", name);
                    continue;
                }

                var success = await _helper.DeleteFileAsync(file);
                if (success)
                {
                    studyDetail.EnrollmentProofs.Remove(file);
                    _logger.LogError("Proof of enrollment {fileName} deleted.", name);
                }
            }
        }

        // Delete study permit or visa
        if (studyDetail.StudyPermitsOrVisas.Any() && command.DeletedStudyPermitsOrVisas.Any())
        {
            foreach (var name in command.DeletedStudyPermitsOrVisas)
            {
                var file = studyDetail.StudyPermitsOrVisas.FirstOrDefault(file => file.FileName == name);
                if (file is null)
                {
                    _logger.LogError("Study permit or visa {fileName} not found.", name);
                    continue;
                }

                var success = await _helper.DeleteFileAsync(file);
                if (success)
                {
                    studyDetail.StudyPermitsOrVisas.Remove(file);
                    _logger.LogInformation("Study permit or visa {fileName} deleted.", name);
                }
            }
        }

        // Delete income proofs
        if (studyDetail.IncomeProofs.Any() && command.DeletedIncomesProofs.Any())
        {
            foreach (var name in command.DeletedIncomesProofs)
            {
                var file = studyDetail.IncomeProofs.FirstOrDefault(file => file.FileName == name);
                if (file is null)
                {
                    _logger.LogError("Proof of incomes {fileName} not found.", name);
                    continue;
                }

                var success = await _helper.DeleteFileAsync(file);
                if (success)
                {
                    studyDetail.IncomeProofs.Remove(file);
                    _logger.LogInformation("Proof of income {fileName} deleted.", name);
                }
            }
        }

        var keyName = GetS3KeyNamePreffix(command.TenantId, command.StudyId);

        // New Enrollment proofs
        foreach (var file in command.NewEnrollmentProofs)
        {
            if (studyDetail.EnrollmentProofs.HaveDuplicates(file, out int count))
                file.RenameWhenIsDuplicated(count);

            var enrollmentProof = await _helper.UploadFileAsync(keyName, file);
            if (enrollmentProof is not null)
            {
                studyDetail.EnrollmentProofs.Add(enrollmentProof);
                _logger.LogInformation("New proof of enrollment saved: {fileName}", enrollmentProof.FileName);
            }
        }

        // New Study permits or visas
        if (command.IsOverseasStudent && command.NewStudyPermitsOrVisas is not null)
            foreach (var file in command.NewStudyPermitsOrVisas)
            {
                if (studyDetail.StudyPermitsOrVisas.HaveDuplicates(file, out int count))
                    file.RenameWhenIsDuplicated(count);

                var studyPermitOrVisa = await _helper.UploadFileAsync(keyName, file);
                if (studyPermitOrVisa is not null)
                {
                    studyDetail.StudyPermitsOrVisas.Add(studyPermitOrVisa);
                    _logger.LogInformation("New study permit or visa saved: {fileName}", studyPermitOrVisa.FileName);
                }
            }

        // New Income proofs
        foreach (var file in command.NewIncomesProofs)
        {
            if (studyDetail.EnrollmentProofs.HaveDuplicates(file, out int count))
                file.RenameWhenIsDuplicated(count);
            
            var incomeProof = await _helper.UploadFileAsync(keyName, file);
            if (incomeProof is not null)
            {
                studyDetail.IncomeProofs.Add(incomeProof);
                _logger.LogInformation("New proof of income saved: {fileName}", incomeProof.FileName);
            }
        }

        await _incomesRepository.Update(tenant);

        _logger.LogInformation("Tenant study detail {studyId} updated.", studyDetail.Id);

        await _repositoryApplicationDetails.VerifyAndUpdateJiraAndVerifiedProfile(_contextFactory, _bus, command.TenantId);
        // Create Event
        var studentDetailUpdatedEvent = new TenantStudentFinancialDetailUpdatedEvent(command.TenantId, "Study Updated");
        var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
        messageOutboxId = await _messageService.SendAsync(notificationContext, studentDetailUpdatedEvent);
    }

    private static string GetS3KeyNamePreffix(Guid tenantId, Guid studyId)
        => $"{tenantId}/{studyId}";
}
