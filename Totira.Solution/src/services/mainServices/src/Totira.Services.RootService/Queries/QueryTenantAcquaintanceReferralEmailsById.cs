using Totira.Support.Application.Queries;

namespace Totira.Services.RootService.Queries
{
    public class QueryTenantAcquaintanceReferralEmailsById : IQuery
    {
        public QueryTenantAcquaintanceReferralEmailsById(Guid id) => Id = id;

        public Guid Id { get; }

    }
}

