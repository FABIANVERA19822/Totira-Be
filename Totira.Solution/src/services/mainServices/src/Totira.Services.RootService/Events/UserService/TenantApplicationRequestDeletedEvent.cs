using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantApplicationRequestDeletedEvent")]
    public class TenantApplicationRequestDeletedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestDeletedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantApplicationRequestDeletedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantApplicationRequestDeletedInfo
        {
            private Guid id;

            public TenantApplicationRequestDeletedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
