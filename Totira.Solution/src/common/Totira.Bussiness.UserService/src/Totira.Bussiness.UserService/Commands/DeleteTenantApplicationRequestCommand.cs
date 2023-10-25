using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("DeleteTenantApplicationRequestCommand")]
    public class DeleteTenantApplicationRequestCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public Guid ApplicationRequestId { get; set; }
    }
}
