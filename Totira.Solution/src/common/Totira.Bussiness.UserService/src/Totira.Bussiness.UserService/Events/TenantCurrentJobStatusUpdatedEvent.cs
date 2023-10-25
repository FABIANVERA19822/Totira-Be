
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantCurrentJobStatusUpdatedEvent")]
    public class TenantCurrentJobStatusUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantCurrentJobStatusUpdatedEvent(Guid id) => Id = id;
    }
}
