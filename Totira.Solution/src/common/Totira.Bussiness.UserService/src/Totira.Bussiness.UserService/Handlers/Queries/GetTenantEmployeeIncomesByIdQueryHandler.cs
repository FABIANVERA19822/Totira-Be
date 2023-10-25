using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantEmployeeIncomesByIdQueryHandler : IQueryHandler<QueryTenantEmployeeIncomesById, GetTenantEmployeeIncomesDto>
    {
        private readonly IRepository<TenantEmployeeIncomes, Guid> _employeeIncomesRepository;

        public GetTenantEmployeeIncomesByIdQueryHandler(IRepository<TenantEmployeeIncomes, Guid> employeeIncomesRepository)
        {
            _employeeIncomesRepository = employeeIncomesRepository;
        }

        public async Task<GetTenantEmployeeIncomesDto> HandleAsync(QueryTenantEmployeeIncomesById query)
        {
            var tenant = await _employeeIncomesRepository.GetByIdAsync(query.TenantId);
            var response = new GetTenantEmployeeIncomesDto(query.TenantId);

            if (tenant is not null)
            {
                response.TenantId = tenant.Id;
                response.IsStudent = tenant.IsStudent;

                if (HaveStudentIncomes(tenant))
                    response.StudyDetails.AddRange(tenant.StudentIncomes!
                        .Select(StudyDetailDto.AdaptFrom));

                if (HaveEmployeeIncomes(tenant))
                {
                    response.CurrentEmployements.AddRange(tenant.EmployeeIncomes!
                        .Where(income => income.IsCurrentlyWorking)
                        .Select(CurrentEmploymentDto.AdaptFrom));

                    response.PastEmployments.AddRange(tenant.EmployeeIncomes!
                        .Where(income => !income.IsCurrentlyWorking)
                        .Select(PastEmploymentDto.AdaptFrom));
                }
            }

            return response;
        }

        private static bool HaveStudentIncomes(TenantEmployeeIncomes entity)
            => entity.StudentIncomes is not null;

        private static bool HaveEmployeeIncomes(TenantEmployeeIncomes entity)
            => entity.EmployeeIncomes is not null && entity.EmployeeIncomes.Any();
    }
}
