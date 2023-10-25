namespace Totira.Services.RootService.Events.UserService
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;
    using Totira.Support.NotificationHub;

    [RoutingKey("TenantCurrentJobStatusUpdatedEvent")]
    public class TenantCurrentJobStatusUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantCurrentJobStatusUpdatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantCurrentJobStatusUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantCurrentJobStatusUpdatedInfo
        {
            private Guid id;

            public TenantCurrentJobStatusUpdatedInfo(Guid id)
            {
                this.id = id;
            }
        }
    }
}