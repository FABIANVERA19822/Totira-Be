using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantContactInformationByTenantId : IQuery
    {
        public Guid TenantId { get; }

        public QueryTenantContactInformationByTenantId(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }
}
