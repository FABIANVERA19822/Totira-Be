using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantApplicationRequestUpdatedEvent")]
    public class TenantApplicationRequestUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestUpdatedEvent(Guid id) => Id = id;
    }
}
