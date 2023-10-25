using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantEmploymentReferenceById : IQuery
    {
        public QueryTenantEmploymentReferenceById(Guid id) => Id = id;
        public Guid Id { get; }
    }
}

