namespace Totira.Bussiness.UserService.Queries
{
    using Totira.Support.Application.Queries;
    public class QueryApplicationTypeByTenantId : IQuery
    {
        public Guid Id { get; }

        public QueryApplicationTypeByTenantId(Guid id) => Id = id;
 
    }
}
