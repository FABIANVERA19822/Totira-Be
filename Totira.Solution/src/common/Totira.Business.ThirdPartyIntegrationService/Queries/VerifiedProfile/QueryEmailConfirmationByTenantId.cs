using Totira.Support.Application.Queries;

namespace Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile
{
    public class QueryEmailConfirmationByTenantId : IQuery
    {
        public QueryEmailConfirmationByTenantId(string tenantId)
        {
            TenantId = tenantId;
        }
        public string TenantId { get; set; }
    }
}
