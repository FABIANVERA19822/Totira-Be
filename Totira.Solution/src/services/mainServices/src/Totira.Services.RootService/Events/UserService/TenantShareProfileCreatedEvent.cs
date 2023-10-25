using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantShareProfileCreatedEvent")]
    public class TenantShareProfileCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public TenantShareProfileCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantShareProfileCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantShareProfileCreatedInfo
        {
            private Guid id;

            public TenantShareProfileCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}

