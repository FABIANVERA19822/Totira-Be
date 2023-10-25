using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantEmploymentReferenceCreatedEvent")]
    public class TenantEmploymentReferenceCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public TenantEmploymentReferenceCreatedEvent(Guid id) => Id = id;

    }
}

