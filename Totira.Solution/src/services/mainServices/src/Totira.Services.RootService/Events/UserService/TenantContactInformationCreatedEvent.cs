using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantContactInformationCreatedEvent")]
    public class TenantContactInformationCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public TenantContactInformationCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantContactInformationCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantContactInformationCreatedInfo
        {
            private Guid id;

            public TenantContactInformationCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
