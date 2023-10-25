using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantApplicationRequestDeletedEvent")]
    public class TenantApplicationRequestDeletedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestDeletedEvent(Guid id) => Id = id;
    }
}
