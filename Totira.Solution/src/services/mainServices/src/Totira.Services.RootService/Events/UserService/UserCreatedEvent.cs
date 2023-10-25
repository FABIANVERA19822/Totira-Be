using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("UserCreatedEvent")]
    public class UserCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public UserCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new UserCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class UserCreatedInfo
        {
            private Guid id;

            public UserCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
