using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantFeedbackViaLandlordById : IQuery
    {
        public Guid Id { get; }

        public QueryTenantFeedbackViaLandlordById(Guid landlordId)
        {
            Id = landlordId;
        }
    }
}

