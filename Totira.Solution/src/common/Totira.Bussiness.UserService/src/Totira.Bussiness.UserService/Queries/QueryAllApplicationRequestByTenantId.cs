using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryAllApplicationRequestByTenantId : IQuery
    {
        public QueryAllApplicationRequestByTenantId(Guid tenantId) => TenantId = tenantId;

        public Guid TenantId { get; }
    }
}
