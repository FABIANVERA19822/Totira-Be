

namespace Totira.Services.RootService.Events.UserService
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;
    using Totira.Support.NotificationHub;

    [RoutingKey(nameof(TenantGroupVerificationDoneEvent))]
    public class TenantGroupVerificationDoneEvent : IEvent, INotification
    {
        public TenantGroupVerificationDoneEvent(Guid mainTenantId)
        {
            MainTenantId = mainTenantId;
        }
        public Guid MainTenantId { get; set; }

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantGroupVerificationDoneInfo(this.MainTenantId);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantGroupVerificationDoneInfo
        {
            private Guid id;

            public TenantGroupVerificationDoneInfo(Guid id)
            {
                this.id = id;
            }
        }
    }
}
