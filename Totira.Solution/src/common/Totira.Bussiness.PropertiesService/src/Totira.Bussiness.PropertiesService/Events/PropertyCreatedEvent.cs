using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.PropertiesService.Events
{

    [RoutingKey("PropertyCreatedEvent")]
    public class PropertyCreatedEvent : IEvent
    {
        public string Id { get; set; }
        public PropertyCreatedEvent(string id) => Id = id;

    }
}
