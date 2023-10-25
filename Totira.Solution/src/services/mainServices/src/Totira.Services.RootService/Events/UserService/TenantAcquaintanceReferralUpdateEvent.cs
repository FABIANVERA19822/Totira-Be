using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantAcquaintanceReferralUpdateEvent")]
    public class TenantAcquaintanceReferralUpdateEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public TenantAcquaintanceReferralUpdateEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantAcquaintanceReferralUpdateInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantAcquaintanceReferralUpdateInfo
        {
            private Guid id;

            public TenantAcquaintanceReferralUpdateInfo(Guid id)
            {
                this.id = id;

            }
        }

    }
}

