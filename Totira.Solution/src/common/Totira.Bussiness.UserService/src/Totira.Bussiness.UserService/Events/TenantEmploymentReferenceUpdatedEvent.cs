using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantEmploymentReferenceUpdatedEvent")]
    public class TenantEmploymentReferenceUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantEmploymentReferenceUpdatedEvent(Guid id) => Id = id;
    }
}

