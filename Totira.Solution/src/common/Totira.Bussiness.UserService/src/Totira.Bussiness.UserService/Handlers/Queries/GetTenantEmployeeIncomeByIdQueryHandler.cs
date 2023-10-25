using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantEmployeeIncomeByIdQueryHandler : IQueryHandler<QueryTenantEmployeeIncomeById, GetTenantEmployeeIncomeDto>
    {
        private readonly IRepository<Domain.TenantEmployeeIncomes, Guid> _employeeIncomesRepository;
        public GetTenantEmployeeIncomeByIdQueryHandler(IRepository<Domain.TenantEmployeeIncomes, Guid> employeeIncomeRepository)
        {
            _employeeIncomesRepository = employeeIncomeRepository;
        }
        public async Task<GetTenantEmployeeIncomeDto> HandleAsync(QueryTenantEmployeeIncomeById query)
        {
            var employeeIncomes = await _employeeIncomesRepository.GetByIdAsync(query.TenantId);

            if (employeeIncomes is null ||
                employeeIncomes.EmployeeIncomes is null ||
                employeeIncomes.EmployeeIncomes.Count == 0)
                return GetTenantEmployeeIncomeDto.Empty(query.IncomeId);

            var employeeIncome = employeeIncomes.EmployeeIncomes
                .FirstOrDefault(income => income.Id == query.IncomeId);

            if (employeeIncome is null)
                return GetTenantEmployeeIncomeDto.Empty(query.IncomeId);

            var response = GetTenantEmployeeIncomeDto.AdaptFrom(employeeIncome);
            return response;
        }
    }
}
