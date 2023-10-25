using Totira.Support.Application.Queries;

namespace Totira.Services.RootService.Queries
{
    public class QueryVerificationsProcessStatusById : IQuery

    {
        public Guid TenantId { get; set; }
        public QueryVerificationsProcessStatusById(Guid id) => TenantId = id;
    }
}
