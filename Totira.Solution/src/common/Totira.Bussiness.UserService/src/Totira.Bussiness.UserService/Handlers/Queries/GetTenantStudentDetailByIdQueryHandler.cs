using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.Common;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries;

public class GetTenantStudentDetailByIdQueryHandler : IQueryHandler<QueryTenantStudentDetailById, GetTenantStudentDetailByIdDto>
{
    private readonly ILogger<GetTenantStudentDetailByIdQueryHandler> _logger;
    private readonly IRepository<TenantEmployeeIncomes, Guid> _tenantEmployeeIncomesRepository;

    public GetTenantStudentDetailByIdQueryHandler(ILogger<GetTenantStudentDetailByIdQueryHandler> logger, IRepository<TenantEmployeeIncomes, Guid> tenantEmployeeIncomesRepository)
    {
        _logger = logger;
        _tenantEmployeeIncomesRepository = tenantEmployeeIncomesRepository;
    }

    public async Task<GetTenantStudentDetailByIdDto> HandleAsync(QueryTenantStudentDetailById query)
    {
        var tenant = await _tenantEmployeeIncomesRepository.GetByIdAsync(query.TenantId);

        if (tenant is null || tenant.StudentIncomes is null || !tenant.StudentIncomes.Any())
            return default!;

        var response = tenant.StudentIncomes
            .Where(income => income.Id == query.StudyId)
            .Select(income => new GetTenantStudentDetailByIdDto(
                tenant.Id,
                income.Id,
                income.UniversityOrInstitute,
                income.Degree,
                income.IsOverseasStudent,
                TenantFileDisplayDto.AdaptFrom(income.EnrollmentProofs),
                TenantFileDisplayDto.AdaptFrom(income.StudyPermitsOrVisas),
                TenantFileDisplayDto.AdaptFrom(income.IncomeProofs)))
            .FirstOrDefault();
            
        throw new NotImplementedException();
    }
}