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
using LanguageExt;

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class UpdateTenantStudentFinancialDetailCommandHandler : IMessageHandler<UpdateTenantStudentFinancialDetailCommand>
{
    private readonly ILogger<UpdateTenantStudentFinancialDetailCommandHandler> _logger;
    private readonly IRepository<TenantEmployeeIncomes, Guid> _incomesRepository;
    private readonly ICommonFunctions _helper;
    private readonly IRepository<TenantVerificationProfile, Guid> _repositoryVerificationProfile;
    private readonly IEventBus _bus;
    private readonly IContextFactory _contextFactory;
    private readonly IMessageService _messageService;

    public UpdateTenantStudentFinancialDetailCommandHandler(
        ILogger<UpdateTenantStudentFinancialDetailCommandHandler> logger,
        IRepository<TenantEmployeeIncomes, Guid> incomesRepository,
        ICommonFunctions helper,
        IEventBus bus,
        IContextFactory contextFactory,
        IRepository<TenantVerificationProfile, Guid> repositoryVerificationProfile,
        IMessageService messageService)
    {
        _logger = logger;
        _incomesRepository = incomesRepository;
        _helper = helper;
        _bus = bus;
        _contextFactory = contextFactory;
        _repositoryVerificationProfile = repositoryVerificationProfile;
        _messageService = messageService;
    }

    public async Task HandleAsync(IContext context, Either<Exception, UpdateTenantStudentFinancialDetailCommand> command)
    {
        await command.MatchAsync(async cmd => {
            Guid? messageOutboxId = null;
            var tenant = await _incomesRepository.GetByIdAsync(cmd.TenantId);

            if (tenant is null)
            {
                _logger.LogError("Tenant not found: {TenantId}.", cmd.TenantId);
                return;
            }
            
            tenant.StudentIncomes ??= new();
            var studyDetail = tenant.StudentIncomes.FirstOrDefault(income => income.Id == cmd.StudyId);

            if (studyDetail is null)
            {
                _logger.LogError("Study detail not found: {StudyId}.", cmd.StudyId);
                return;
            }
            
            studyDetail.UpdateInformation(
                cmd.UniversityOrInstitute,
                cmd.Degree,
                cmd.IsOverseasStudent);

            _logger.LogInformation("Study detail info updated.");

            // Delete enrollment proof
            if (studyDetail.EnrollmentProofs.Any() && cmd.DeletedEnrollmentProofs.Any())
            {
                foreach (var name in cmd.DeletedEnrollmentProofs)
                {
                    var file = studyDetail.EnrollmentProofs.FirstOrDefault(file => file.FileName == name);
                    if (file is null)
                    {
                        _logger.LogError("Proof of enrollment {FileName} not found.", name);
                        continue;
                    }

                    var success = await _helper.DeleteFileAsync(file);
                    if (success)
                    {
                        studyDetail.EnrollmentProofs.Remove(file);
                        _logger.LogError("Proof of enrollment {FileName} deleted.", name);
                    }
                }
            }

            // Delete study permit or visa
            if (studyDetail.StudyPermitsOrVisas.Any() && cmd.DeletedStudyPermitsOrVisas.Any())
            {
                foreach (var name in cmd.DeletedStudyPermitsOrVisas)
                {
                    var file = studyDetail.StudyPermitsOrVisas.FirstOrDefault(file => file.FileName == name);
                    if (file is null)
                    {
                        _logger.LogError("Study permit or visa {FileName} not found.", name);
                        continue;
                    }

                    var success = await _helper.DeleteFileAsync(file);
                    if (success)
                    {
                        studyDetail.StudyPermitsOrVisas.Remove(file);
                        _logger.LogInformation("Study permit or visa {FileName} deleted.", name);
                    }
                }
            }

            // Delete income proofs
            if (studyDetail.IncomeProofs.Any() && cmd.DeletedIncomesProofs.Any())
            {
                foreach (var name in cmd.DeletedIncomesProofs)
                {
                    var file = studyDetail.IncomeProofs.FirstOrDefault(file => file.FileName == name);
                    if (file is null)
                    {
                        _logger.LogError("Proof of incomes {FileName} not found.", name);
                        continue;
                    }

                    var success = await _helper.DeleteFileAsync(file);
                    if (success)
                    {
                        studyDetail.IncomeProofs.Remove(file);
                        _logger.LogInformation("Proof of income {FileName} deleted.", name);
                    }
                }
            }

            var keyName = GetS3KeyNamePreffix(cmd.TenantId, cmd.StudyId);

            // New Enrollment proofs
            foreach (var file in cmd.NewEnrollmentProofs)
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
            if (cmd.IsOverseasStudent && cmd.NewStudyPermitsOrVisas is not null)
                foreach (var file in cmd.NewStudyPermitsOrVisas)
                {
                    if (studyDetail.StudyPermitsOrVisas.HaveDuplicates(file, out int count))
                        file.RenameWhenIsDuplicated(count);

                    var studyPermitOrVisa = await _helper.UploadFileAsync(keyName, file);
                    if (studyPermitOrVisa is not null)
                    {
                        studyDetail.StudyPermitsOrVisas.Add(studyPermitOrVisa);
                        _logger.LogInformation("New study permit or visa saved: {FileName}", studyPermitOrVisa.FileName);
                    }
                }

            // New Income proofs
            foreach (var file in cmd.NewIncomesProofs)
            {
                if (studyDetail.EnrollmentProofs.HaveDuplicates(file, out int count))
                    file.RenameWhenIsDuplicated(count);
                
                var incomeProof = await _helper.UploadFileAsync(keyName, file);
                if (incomeProof is not null)
                {
                    studyDetail.IncomeProofs.Add(incomeProof);
                    _logger.LogInformation("New proof of income saved: {FileName}", incomeProof.FileName);
                }
            }

            await _incomesRepository.Update(tenant);

            _logger.LogInformation("Tenant study detail {StudyId} updated.", studyDetail.Id);

            //await _repositoryApplicationDetails.VerifyAndUpdateJiraAndVerifiedProfile(_contextFactory, _bus, cmd.TenantId);
            await _repositoryVerificationProfile.UnverifyTenant(cmd.TenantId, _contextFactory, _bus);
            // Create Event
            var studentDetailUpdatedEvent = new TenantStudentFinancialDetailUpdatedEvent(cmd.TenantId, "Study Updated");
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            messageOutboxId = await _messageService.SendAsync(notificationContext, studentDetailUpdatedEvent);
        }, ex => throw ex);
    }

    private static string GetS3KeyNamePreffix(Guid tenantId, Guid studyId)
        => $"{tenantId}/{studyId}";
}
