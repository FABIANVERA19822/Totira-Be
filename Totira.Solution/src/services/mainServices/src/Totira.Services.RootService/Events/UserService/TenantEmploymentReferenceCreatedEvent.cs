using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantEmploymentReferenceCreatedEvent")]
    public class TenantEmploymentReferenceCreatedEvent : IEvent , INotification
    {
        public Guid Id { get; set; }
        public TenantEmploymentReferenceCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantEmploymentReferenceCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantEmploymentReferenceCreatedInfo
        {
            private Guid id;

            public TenantEmploymentReferenceCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }

    }
}

