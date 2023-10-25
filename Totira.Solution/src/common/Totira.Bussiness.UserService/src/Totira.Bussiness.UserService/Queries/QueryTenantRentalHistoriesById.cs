using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantRentalHistoriesById : IQuery
    {
        public Guid Id { get; }
        public QueryTenantRentalHistoriesById(Guid id) => Id = id;

    }
}

