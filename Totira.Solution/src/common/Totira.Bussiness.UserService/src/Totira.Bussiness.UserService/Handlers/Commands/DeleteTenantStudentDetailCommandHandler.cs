using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class DeleteTenantStudentDetailCommandHandler : IMessageHandler<DeleteTenantStudentDetailCommand>
{
    private readonly ILogger<DeleteTenantStudentDetailCommandHandler> _logger;
    private readonly IRepository<TenantEmployeeIncomes, Guid> _tenantEmployeeIncomesRepository;
    private readonly ICommonFunctions _helper;

    public DeleteTenantStudentDetailCommandHandler(
        ILogger<DeleteTenantStudentDetailCommandHandler> logger,
        IRepository<TenantEmployeeIncomes, Guid> tenantEmployeeIncomesRepository,
        ICommonFunctions helper)
    {
        _logger = logger;
        _tenantEmployeeIncomesRepository = tenantEmployeeIncomesRepository;
        _helper = helper;
    }

    public async Task HandleAsync(IContext context, DeleteTenantStudentDetailCommand message)
    {
        var tenantIncomes = await _tenantEmployeeIncomesRepository.GetByIdAsync(message.TenantId);
        if (tenantIncomes is null || tenantIncomes.StudentIncomes is null || !tenantIncomes.StudentIncomes.Any())
        {
            _logger.LogError("Student incomes for tenant {tenantId} not found.", message.TenantId);
            return;
        }

        var studyDetails = tenantIncomes.StudentIncomes.FirstOrDefault(income => income.Id == message.StudyId);
        if (studyDetails is null)
        {
            _logger.LogError("Student detail {studyId} not found.", message.StudyId);
            return;
        }

        List<bool> enrollmentProofsResults = new();
        List<bool> studyPermitsOrVisasResults = new();
        List<bool> incomeProofsResults = new();

        foreach (var file in studyDetails.EnrollmentProofs)
        {
            var result = await _helper.DeleteFileAsync(file);
            enrollmentProofsResults.Add(result);
        }

        foreach (var file in studyDetails.StudyPermitsOrVisas)
        {
            var result = await _helper.DeleteFileAsync(file);
            studyPermitsOrVisasResults.Add(result);
        }

        foreach (var file in studyDetails.IncomeProofs)
        {
            var result = await _helper.DeleteFileAsync(file);
            incomeProofsResults.Add(result);
        }

        if (enrollmentProofsResults.All(deleted => deleted)  &&
            studyPermitsOrVisasResults.All(deleted => deleted) &&
            incomeProofsResults.All(deleted => deleted))
        {
            tenantIncomes.StudentIncomes.Remove(studyDetails);
            _logger.LogInformation("Student detail {studyId} deleted.", message.StudyId);

            tenantIncomes.IsStudent = IfStillHavingStudentIncomesWithoutEmploymentIncomes(tenantIncomes);

            await _tenantEmployeeIncomesRepository.Update(tenantIncomes);
            _logger.LogInformation("Tenant incomes updated.");
        }
        else
            _logger.LogError("Student detail {studyId} delete failed.", message.StudyId);
    }

    private static bool IfStillHavingStudentIncomesWithoutEmploymentIncomes(TenantEmployeeIncomes tenantIncomes) =>
        tenantIncomes.StudentIncomes is not null &&
        tenantIncomes.StudentIncomes.Any() &&
        (tenantIncomes.EmployeeIncomes is null || !tenantIncomes.EmployeeIncomes.Any());
}