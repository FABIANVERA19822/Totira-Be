namespace Totira.Services.RootService.Events.UserService
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;
    using Totira.Support.NotificationHub;

    [RoutingKey("TenantApplicationTypeCreatedEvent")]
    public class TenantApplicationTypeCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantApplicationTypeCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantApplicationTypeCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantApplicationTypeCreatedInfo
        {
            private Guid id;

            public TenantApplicationTypeCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
