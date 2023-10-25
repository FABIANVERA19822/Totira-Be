using Totira.Support.Application.Events;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantGroupApplicationShareProfileCreatedEvent")]
    public class TenantGroupApplicationShareProfileCreatedEvent : BaseValidatedEvent
    {
        public Guid Id { get; set; }
        public TenantGroupApplicationShareProfileCreatedEvent(Guid id) => Id = id;
        public TenantGroupApplicationShareProfileCreatedEvent()
        {
            Id = Guid.Empty;
        }
    }
}
