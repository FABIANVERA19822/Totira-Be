namespace Totira.Services.RootService.Events.UserService
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;
    using Totira.Support.NotificationHub;

    [RoutingKey("TenantApplicationTypeUpdatedEvent")]
    public class TenantApplicationTypeUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantApplicationTypeUpdatedEvent(Guid id) => Id = id;
        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantApplicationTypeUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantApplicationTypeUpdatedInfo
        {
            private Guid id;

            public TenantApplicationTypeUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }

    }
}
