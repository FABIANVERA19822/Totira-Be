using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{

    [RoutingKey("TenantProfileImageUpdatedEvent")]
    public class TenantProfileImageUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantProfileImageUpdatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantProfileImageUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantProfileImageUpdatedInfo
        {
            private Guid id;

            public TenantProfileImageUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
