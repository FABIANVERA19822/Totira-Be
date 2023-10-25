using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantGroupInventeesByTenantId : IQuery
    {
        public QueryTenantGroupInventeesByTenantId(Guid tenantId) => TenantId = tenantId;
        public Guid TenantId { get; set; }
    }
}
