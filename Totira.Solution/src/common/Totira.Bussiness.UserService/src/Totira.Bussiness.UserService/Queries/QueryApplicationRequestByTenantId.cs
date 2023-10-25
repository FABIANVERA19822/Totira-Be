using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryApplicationRequestByTenantId : IQuery
    {
        public QueryApplicationRequestByTenantId(Guid tenantId, Guid? applicationId = null) { TenantId = tenantId; ApplicationId = applicationId; }

        public Guid TenantId { get; }
        public Guid? ApplicationId { get; set; }
    }
}
