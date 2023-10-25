using Totira.Support.Application.Queries;

namespace Totira.Business.ThirdPartyIntegrationService.Queries.Jira
{
    public class QueryJiraTicketEmployementByTenantId : IQuery
    {
        public QueryJiraTicketEmployementByTenantId(Guid tenantId)
        {
            TenantId = tenantId;
        }
        public Guid TenantId { get; set; }
    }
}
