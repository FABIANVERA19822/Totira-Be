using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantPersonalInformationUpdatedEvent")]
    public class TenantPersonalInformationUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public string Message { get; }

        public TenantPersonalInformationUpdatedEvent(Guid id, string message)
        {
            Id = id;
            this.Message = message;
        }

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantPersonalInformatioUpdatedInfo(this.Id, this.Message);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantPersonalInformatioUpdatedInfo
        {
            private Guid Id { get; set; }
            public string Message { get; }

            public TenantPersonalInformatioUpdatedInfo(Guid id, string message)
            {
                this.Id = id;
                this.Message = message;
            }
        }
    }
}
