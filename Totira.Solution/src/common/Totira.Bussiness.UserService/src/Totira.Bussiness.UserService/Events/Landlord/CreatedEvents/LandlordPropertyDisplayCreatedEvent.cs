using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events.Landlord.CreatedEvents
{
    [RoutingKey("LandlordPropertyDisplayCreatedEvent")]
    public class LandlordPropertyDisplayCreatedEvent : BaseValidatedEvent
    {
        public Guid Id { get; set; }
        public LandlordPropertyDisplayCreatedEvent(Guid id, bool success) { Id = id; }
        public LandlordPropertyDisplayCreatedEvent() { Id = Guid.Empty; }
    }
}
