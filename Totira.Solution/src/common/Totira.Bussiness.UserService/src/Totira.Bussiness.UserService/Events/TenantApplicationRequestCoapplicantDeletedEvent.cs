using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantApplicationRequestCoapplicantDeletedEvent")]
    public class TenantApplicationRequestCoapplicantDeletedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestCoapplicantDeletedEvent(Guid id) => Id = id;
    }
}
