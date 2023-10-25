using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryApplicationRequestbyInvitationId: IQuery
    {
        public QueryApplicationRequestbyInvitationId(Guid id) { InvitationId = id;}

        public Guid InvitationId { get; set; }
    }
}
