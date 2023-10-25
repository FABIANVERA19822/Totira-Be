using Totira.Support.Application.Queries;

namespace Totira.Business.ThirdPartyIntegrationService.Queries.Certn
{
    public class QueryTenantApplicationById : IQuery
    {
        public QueryTenantApplicationById(string id)
        {
            Id = id;
        }
        public string Id { get; set; }

    }
}
