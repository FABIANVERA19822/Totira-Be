using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantPersonalInformationCreatedEvent")]
    public class TenantPersonalInformationCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantPersonalInformationCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantPersonalInformationCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantPersonalInformationCreatedInfo
        {
            private Guid id;

            public TenantPersonalInformationCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
