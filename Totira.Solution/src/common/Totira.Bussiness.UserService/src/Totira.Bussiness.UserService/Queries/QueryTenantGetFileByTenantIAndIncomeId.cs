using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantGetFileByTenantIAndIncomeId : IQuery
    {
        public QueryTenantGetFileByTenantIAndIncomeId()
        {
            
        }
        public QueryTenantGetFileByTenantIAndIncomeId(Guid tenantId, Guid incomeId)
        {
            TenantId = tenantId;
            IncomeId = incomeId;
        }
        public Guid TenantId { get; set; }
        public Guid IncomeId { get; set; }
    }
}
