using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("UpdateApplicationRequestInvitationCommand")]
    public class UpdateApplicationRequestInvitationCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public string CoapplicantEmail { get; set; }
        public bool IsActive { get; set; }
        public Guid ApplicationRequestId { get; set; }
    }
}
