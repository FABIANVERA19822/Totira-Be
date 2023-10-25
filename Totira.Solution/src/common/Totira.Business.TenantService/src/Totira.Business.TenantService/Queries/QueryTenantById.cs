using Totira.Support.Application.Queries;

namespace Totira.Business.TenantService.Queries
{
    public class QueryTenantById : IQuery
    {
        public Guid Id { get; set; }

        public QueryTenantById(Guid id)
        {
            Id = id;
        }
    }
}
