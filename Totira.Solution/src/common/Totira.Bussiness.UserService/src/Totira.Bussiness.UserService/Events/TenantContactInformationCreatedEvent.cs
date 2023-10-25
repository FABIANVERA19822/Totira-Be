using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantContactInformationCreatedEvent")]
    public class TenantContactInformationCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public TenantContactInformationCreatedEvent(Guid id) => Id = id;
    }
}
