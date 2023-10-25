using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantApplicationRequestCreatedEvent")]
    public class TenantApplicationRequestCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestCreatedEvent(Guid id) => Id = id;
    }
}
