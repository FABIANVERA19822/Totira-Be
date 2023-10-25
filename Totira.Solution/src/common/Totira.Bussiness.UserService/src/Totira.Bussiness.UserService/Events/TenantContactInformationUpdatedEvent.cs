using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantContactInfoUpdatedEvent")]
    public class TenantContactInformationUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantContactInformationUpdatedEvent(Guid id) => Id = id;
    }
}
