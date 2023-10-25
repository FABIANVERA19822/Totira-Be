using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantApplicationRequestUpdatedEvent")]
    public class TenantApplicationRequestUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestUpdatedEvent(Guid id) => Id = id;
        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantApplicationRequestUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantApplicationRequestUpdatedInfo
        {
            private Guid id;

            public TenantApplicationRequestUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
