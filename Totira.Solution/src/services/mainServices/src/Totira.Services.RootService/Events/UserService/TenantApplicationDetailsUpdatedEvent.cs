using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{


    [RoutingKey("TenantApplicationDetailsUpdatedEvent")]
    public class TenantApplicationDetailsUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantApplicationDetailsUpdatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantApplicationDetailsUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantApplicationDetailsUpdatedInfo
        {
            private Guid id;

            public TenantApplicationDetailsUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }

    }
}
