using Totira.Support.Application.Queries;

namespace Totira.Business.ThirdPartyIntegrationService.Queries.Certn
{
    public class QueryApplicationByTenantId : IQuery
    {
        public QueryApplicationByTenantId(string tenantId)
        {
            TenantId = tenantId;
        }
        public string TenantId { get; set; }
    }
}
