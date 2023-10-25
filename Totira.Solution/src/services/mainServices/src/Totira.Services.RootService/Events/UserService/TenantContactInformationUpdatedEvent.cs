using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantContactInfoUpdatedEvent")]
    public class TenantContactInformationUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantContactInformationUpdatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantContactInformationUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantContactInformationUpdatedInfo
        {
            private Guid id;

            public TenantContactInformationUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }


    }
}
