using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
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
