using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantApplicationRequestGuarantorDeletedEvent")]
    public class TenantApplicationRequestGuarantorDeletedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestGuarantorDeletedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantApplicationRequestGuarantorDeletedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantApplicationRequestGuarantorDeletedInfo
        {
            private Guid id;

            public TenantApplicationRequestGuarantorDeletedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
