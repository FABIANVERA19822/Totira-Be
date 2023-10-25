using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{


    [RoutingKey("TenantApplicationDetailsUpdatedEvent")]
    public class TenantApplicationDetailsUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantApplicationDetailsUpdatedEvent(Guid id) => Id = id;

    }
}
