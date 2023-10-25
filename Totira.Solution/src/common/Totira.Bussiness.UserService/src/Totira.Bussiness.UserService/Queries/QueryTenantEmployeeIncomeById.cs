using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public record QueryTenantEmployeeIncomeById(Guid TenantId, Guid IncomeId) : IQuery;
}
