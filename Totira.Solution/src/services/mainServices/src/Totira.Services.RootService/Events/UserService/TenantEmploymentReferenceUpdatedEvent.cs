using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantEmploymentReferenceUpdatedEvent")]
    public class TenantEmploymentReferenceUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantEmploymentReferenceUpdatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantEmploymentReferenceUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantEmploymentReferenceUpdatedInfo
        {
            private Guid id;

            public TenantEmploymentReferenceUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}

