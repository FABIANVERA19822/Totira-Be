using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("ApplicationRequestInvitationResponseCommand")]
    public class ApplicationRequestInvitationResponseCommand : ICommand
    {
        public Guid InvitationId { get; set; }
        public bool Accepted { get; set; }
    }
}
