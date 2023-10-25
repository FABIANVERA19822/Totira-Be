using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    
    public class QueryInvitationsByApplicationRequestById : IQuery
    {
        public QueryInvitationsByApplicationRequestById(Guid applicationRequestId) { ApplicationRequestId = applicationRequestId; }

        public Guid ApplicationRequestId { get; set; }
    }
}
