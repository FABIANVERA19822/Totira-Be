using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("UserCreatedEvent")]
    public class UserCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public UserCreatedEvent(Guid id) => Id = id;

    }
}
