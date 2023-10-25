
using Totira.Support.Application.Queries;

namespace Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile
{
    public class QueryVerifiedProfileByTenantId : IQuery
    {
        public QueryVerifiedProfileByTenantId(string tenantId)
        {
            TenantId = tenantId;
        }
        public string TenantId { get; set; }
    }
}
