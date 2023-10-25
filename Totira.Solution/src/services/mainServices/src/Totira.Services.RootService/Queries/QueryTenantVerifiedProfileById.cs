using Totira.Support.Application.Queries;

namespace Totira.Services.RootService.Queries
{
    public class QueryTenantVerifiedProfileById : IQuery
    {
        public QueryTenantVerifiedProfileById(Guid id) => Id = id;

        public Guid Id { get; }

    }
}
