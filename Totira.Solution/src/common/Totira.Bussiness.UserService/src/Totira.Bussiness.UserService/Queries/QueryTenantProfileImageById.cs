namespace Totira.Bussiness.UserService.Queries
{
    using Totira.Support.Application.Queries;
    public class QueryTenantProfileImageById : IQuery
    {
        public QueryTenantProfileImageById(Guid id) => Id = id;
        public Guid Id { get; }
    }
}
