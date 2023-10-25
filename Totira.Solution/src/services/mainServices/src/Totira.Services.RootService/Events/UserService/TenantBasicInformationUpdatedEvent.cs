using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantBasicInformationUpdatedEvent")]
    public class TenantBasicInformationUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantBasicInformationUpdatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantBasicInformationUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantBasicInformationUpdatedInfo
        {
            private Guid id;

            public TenantBasicInformationUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
