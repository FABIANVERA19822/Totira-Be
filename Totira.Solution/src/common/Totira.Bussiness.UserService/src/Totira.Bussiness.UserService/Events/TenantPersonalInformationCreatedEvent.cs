using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantPersonalInformationCreatedEvent")]
    public class TenantPersonalInformationCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantPersonalInformationCreatedEvent(Guid id) => Id = id;

    }
}
