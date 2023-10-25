using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("ApplicationRequestInvitationResponseCommand")]
    public class ApplicationRequestInvitationResponseCommand : ICommand
    {
        public Guid InvitationId { get; set; }
        public bool Accepted { get; set; }
    }
}
