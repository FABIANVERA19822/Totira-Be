using Totira.Support.Application.Queries;

namespace Totira.Business.ThirdPartyIntegrationService.Queries.Persona
{
    public class QueryPersonaInquiryByTenantId : IQuery
    {
        public QueryPersonaInquiryByTenantId(Guid tenantId)
        {
            TenantId = tenantId;
        }
        public Guid TenantId { get; set; }
    }

}

