using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantApplicationDetailsCreatedEvent")]
    public class TenantApplicationDetailsCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantApplicationDetailsCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantApplicationDetailsCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantApplicationDetailsCreatedInfo
        {
            private Guid id;

            public TenantApplicationDetailsCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }

    }
}
