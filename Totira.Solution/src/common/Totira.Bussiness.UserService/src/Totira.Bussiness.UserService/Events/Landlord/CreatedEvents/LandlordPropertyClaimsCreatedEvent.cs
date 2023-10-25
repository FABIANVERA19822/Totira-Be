using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events.Landlord.CreatedEvents
{
    [RoutingKey("LandlordPropertyClaimsCreatedEvent")]
    internal class LandlordPropertyClaimsCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }
        public LandlordPropertyClaimsCreatedEvent(Guid id, bool success) { Id = id; Success = success; }
    }
}