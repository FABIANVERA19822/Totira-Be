using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{


    [RoutingKey("TenantBasicInformationCreatedEvent")]
    public class TenantBasicInformationCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantBasicInformationCreatedEvent(Guid id) => Id = id;

    }
}
