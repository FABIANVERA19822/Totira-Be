using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantApplicationRequestCreatedEvent")]
    public class TenantApplicationRequestCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestCreatedEvent(Guid id) => Id = id;
        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantApplicationRequestCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantApplicationRequestCreatedInfo
        {
            private Guid id;

            public TenantApplicationRequestCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
