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

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class UpdateTenantStudentFinancialDetailCommandHandler : IMessageHandler<UpdateTenantStudentFinancialDetailCommand>
{
    private readonly ILogger<UpdateTenantStudentFinancialDetailCommandHandler> _logger;
    private readonly IRepository<TenantEmployeeIncomes, Guid> _incomesRepository;
    private readonly ICommonFunctions _helper;
    private readonly IRepository<TenantApplicationDetails, Guid> _repositoryApplicationDetails;
    private readonly IEventBus _bus;
    private readonly IContextFactory _contextFactory;

    public UpdateTenantStudentFinancialDetailCommandHandler(
        ILogger<UpdateTenantStudentFinancialDetailCommandHandler> logger,
        IRepository<TenantEmployeeIncomes, Guid> incomesRepository,
        ICommonFunctions helper,
        IEventBus bus,
        IContextFactory contextFactory,
        IRepository<TenantApplicationDetails, Guid> repositoryApplicationDetails)
    {
        _logger = logger;
        _incomesRepository = incomesRepository;
        _helper = helper;
        _bus = bus;
        _contextFactory = contextFactory;
        _repositoryApplicationDetails = repositoryApplicationDetails;
    }

    public async Task HandleAsync(IContext context, UpdateTenantStudentFinancialDetailCommand message)
    {
        var tenant = await _incomesRepository.GetByIdAsync(message.TenantId);

        if (tenant is null)
        {
            _logger.LogError("Tenant not found: {tenantId}.", message.TenantId);
            return;
        }
        
        tenant.IsStudent = !(tenant.EmployeeIncomes is not null && tenant.EmployeeIncomes.Any());
        tenant.StudentIncomes ??= new();
        var studyDetail = tenant.StudentIncomes.FirstOrDefault(income => income.Id == message.StudyId);

        if (studyDetail is null)
        {
            _logger.LogError("Study detail not found: {studyId}.", message.StudyId);
            return;
        }
        
        studyDetail.UpdateInformation(
            message.UniversityOrInstitute,
            message.Degree,
            message.IsOverseasStudent);

        _logger.LogInformation("Study detail info updated.");

        // Delete enrollment proof
        if (studyDetail.EnrollmentProofs.Any() && message.DeletedEnrollmentProofs.Any())
        {
            foreach (var name in message.DeletedEnrollmentProofs)
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
        if (studyDetail.StudyPermitsOrVisas.Any() && message.DeletedStudyPermitsOrVisas.Any())
        {
            foreach (var name in message.DeletedStudyPermitsOrVisas)
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
        if (studyDetail.IncomeProofs.Any() && message.DeletedIncomesProofs.Any())
        {
            foreach (var name in message.DeletedIncomesProofs)
            {
                var file = studyDetail.EnrollmentProofs.FirstOrDefault(file => file.FileName == name);
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

        var keyName = GetS3KeyNamePreffix(message.TenantId, message.StudyId);

        // New Enrollment proofs
        foreach (var file in message.NewEnrollmentProofs)
        {
            var enrollmentProof = await _helper.UploadFileAsync(keyName, file);
            if (enrollmentProof is not null)
            {
                studyDetail.StudyPermitsOrVisas.Add(enrollmentProof);
                _logger.LogInformation("New proof of enrollment saved: {fileName}", enrollmentProof.FileName);
            }
        }

        // New Study permits or visas
        if (message.IsOverseasStudent && message.NewStudyPermitsOrVisas is not null)
            foreach (var file in message.NewStudyPermitsOrVisas)
            {
                var studyPermitOrVisa = await _helper.UploadFileAsync(keyName, file);
                if (studyPermitOrVisa is not null)
                {
                    studyDetail.StudyPermitsOrVisas.Add(studyPermitOrVisa);
                    _logger.LogInformation("New study permit or visa saved: {fileName}", studyPermitOrVisa.FileName);
                }
            }

        // New Income proofs
        foreach (var file in message.NewIncomesProofs)
        {
            var incomeProof = await _helper.UploadFileAsync(keyName, file);
            if (incomeProof is not null)
            {
                studyDetail.IncomeProofs.Add(incomeProof);
                _logger.LogInformation("New proof of income saved: {fileName}", incomeProof.FileName);
            }
        }

        await _incomesRepository.Update(tenant);

        _logger.LogInformation("Tenant study detail {studyId} updated.", studyDetail.Id);

        await _repositoryApplicationDetails.VerifyAndUpdateJiraAndVerifiedProfile(_contextFactory, _bus, message.TenantId);
    }

    private static string GetS3KeyNamePreffix(Guid tenantId, Guid studyId)
        => $"{tenantId}/{studyId}";
}
