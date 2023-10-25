using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantFeedbackViaLandlordCreatedEvent")]
    public class TenantFeedbackViaLandlordCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantFeedbackViaLandlordCreatedEvent(Guid id) => Id = id;
    }
}
