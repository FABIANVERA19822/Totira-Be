using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events.Landlord.CreatedEvents
{
    [RoutingKey("LandlordIdentityCreatedEvent")]
    public class LandlordIdentityCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }
        public LandlordIdentityCreatedEvent(Guid id, bool success) 
        { 
            Id = id; 
            Success = success; 
        }
    }
}
