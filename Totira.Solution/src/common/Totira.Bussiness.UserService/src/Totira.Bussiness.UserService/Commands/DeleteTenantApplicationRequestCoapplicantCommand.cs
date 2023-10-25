using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("DeleteTenantApplicationRequestCoapplicantCommand")]
    public class DeleteTenantApplicationRequestCoapplicantCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public Guid ApplicationRequestId { get; set; }

        public Guid CoapplicantId { get; set; }
    }
}
