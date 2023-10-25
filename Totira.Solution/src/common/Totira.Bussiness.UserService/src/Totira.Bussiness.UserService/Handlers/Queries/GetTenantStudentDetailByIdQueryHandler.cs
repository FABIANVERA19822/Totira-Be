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
        var tenantIncomes = await _tenantEmployeeIncomesRepository.GetByIdAsync(query.TenantId);

        if (tenantIncomes is null || tenantIncomes.StudentIncomes is null || !tenantIncomes.StudentIncomes.Any())
            return default!;

        var studyDetail = tenantIncomes.StudentIncomes.FirstOrDefault(x => x.Id == query.StudyId);

        if (studyDetail is null)
            return default!;

        var response = new GetTenantStudentDetailByIdDto(
                tenantIncomes.Id,
                studyDetail.Id,
                studyDetail.UniversityOrInstitute,
                studyDetail.Degree,
                studyDetail.IsOverseasStudent,
                TenantFileDisplayDto.AdaptFrom(studyDetail.EnrollmentProofs),
                TenantFileDisplayDto.AdaptFrom(studyDetail.StudyPermitsOrVisas),
                TenantFileDisplayDto.AdaptFrom(studyDetail.IncomeProofs));
            
        return response;
    }
}