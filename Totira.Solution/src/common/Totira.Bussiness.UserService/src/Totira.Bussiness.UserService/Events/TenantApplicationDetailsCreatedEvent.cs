using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantApplicationDetailsCreatedEvent")]
    public class TenantApplicationDetailsCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantApplicationDetailsCreatedEvent(Guid id) => Id = id;

    }
}
