using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events.Landlord.CreatedEvents
{
    [RoutingKey("LandlordBasicInformationCreatedEvent")]
    public class LandlordBasicInformationCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public LandlordBasicInformationCreatedEvent(Guid id) => Id = id;

    }
}
