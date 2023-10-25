using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantAcquaintanceReferralById : IQuery
    {
        public QueryTenantAcquaintanceReferralById(Guid id) => Id = id;

        public Guid Id { get; }
    }
}

