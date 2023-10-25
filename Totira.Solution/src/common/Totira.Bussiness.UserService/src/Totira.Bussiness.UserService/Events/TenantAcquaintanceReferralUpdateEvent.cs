using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantAcquaintanceReferralUpdateEvent")]
    public class TenantAcquaintanceReferralUpdateEvent : IEvent
    {
        public Guid Id { get; set; }
        public TenantAcquaintanceReferralUpdateEvent(Guid id) => Id = id;

    }
}

