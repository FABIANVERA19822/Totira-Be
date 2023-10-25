
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryCurrentJobStatusByTenantId : IQuery
    {
        public Guid Id { get; }
        public QueryCurrentJobStatusByTenantId(Guid id) => Id = id;
    }
}
