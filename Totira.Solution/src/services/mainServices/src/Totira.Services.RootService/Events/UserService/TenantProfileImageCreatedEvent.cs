using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantProfileImageCreatedEvent")]
    public class TenantProfileImageCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantProfileImageCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantProfileImageCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantProfileImageCreatedInfo
        {
            private Guid id;

            public TenantProfileImageCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
