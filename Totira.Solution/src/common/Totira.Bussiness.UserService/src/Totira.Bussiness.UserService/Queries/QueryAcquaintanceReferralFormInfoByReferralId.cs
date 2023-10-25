using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryAcquaintanceReferralFormInfoByReferralId : IQuery
    {
        public QueryAcquaintanceReferralFormInfoByReferralId(Guid id) => Id = id;

        public Guid Id { get; }
    }
}
