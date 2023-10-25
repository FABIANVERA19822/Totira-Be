using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantPersonalInformationUpdatedEvent")]
    public class TenantPersonalInformatioUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantPersonalInformatioUpdatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantPersonalInformatioUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantPersonalInformatioUpdatedInfo
        {
            private Guid id;

            public TenantPersonalInformatioUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
