namespace Totira.Services.RootService.Events.UserService
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;
    using Totira.Support.NotificationHub;

    [RoutingKey("TenantApplicationRequestSendEmailsUpdatedEvent")]
    public class TenantApplicationRequestSendEmailsUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestSendEmailsUpdatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantApplicationRequestSendEmailsUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantApplicationRequestSendEmailsUpdatedInfo
        {
            private Guid id;

            public TenantApplicationRequestSendEmailsUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
