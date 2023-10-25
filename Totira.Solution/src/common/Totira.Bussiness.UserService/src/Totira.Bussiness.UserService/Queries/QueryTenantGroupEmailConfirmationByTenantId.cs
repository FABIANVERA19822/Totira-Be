
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantGroupEmailConfirmationByTenantId : IQuery
    {
        public QueryTenantGroupEmailConfirmationByTenantId(Guid tenantId) => TenantId = tenantId;
        public Guid TenantId { get; set; }
    }
}
