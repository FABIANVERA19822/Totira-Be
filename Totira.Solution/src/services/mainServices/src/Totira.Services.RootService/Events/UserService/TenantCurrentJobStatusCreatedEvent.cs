

namespace Totira.Services.RootService.Events.UserService
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;
    using Totira.Support.NotificationHub;

    [RoutingKey("TenantApplicationTypeCreatedEvent")]
    public class TenantCurrentJobStatusCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantCurrentJobStatusCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantCurrentJobStatusCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantCurrentJobStatusCreatedInfo
        {
            private Guid id;

            public TenantCurrentJobStatusCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
