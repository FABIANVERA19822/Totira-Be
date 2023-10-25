using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantBasicInformationUpdatedEvent")]
    public class TenantBasicInformationUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantBasicInformationUpdatedEvent(Guid id) => Id = id;

    }
}
