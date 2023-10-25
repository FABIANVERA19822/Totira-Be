using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantApplicationDetailsById : IQuery
    {
        public Guid Id { get; }

        public QueryTenantApplicationDetailsById(Guid id) => Id = id;


    }
}
