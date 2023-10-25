using Totira.Support.Application.Queries;

namespace Totira.Services.RootService.Queries
{
    public class QueryUserById : IQuery
    {
        public QueryUserById(Guid id) => Id = id;

        public Guid Id { get; }
    }
}
