using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantApplicationRequestGuarantorDeletedEvent")]
    public class TenantApplicationRequestGuarantorDeletedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestGuarantorDeletedEvent(Guid id) => Id = id;
    }
}
