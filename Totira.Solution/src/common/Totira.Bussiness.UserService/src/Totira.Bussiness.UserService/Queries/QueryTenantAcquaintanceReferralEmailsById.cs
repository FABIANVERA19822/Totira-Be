using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantAcquaintanceReferralEmailsById : IQuery
    {
        public QueryTenantAcquaintanceReferralEmailsById(Guid id) => Id = id;

        public Guid Id { get; }
    }
}

