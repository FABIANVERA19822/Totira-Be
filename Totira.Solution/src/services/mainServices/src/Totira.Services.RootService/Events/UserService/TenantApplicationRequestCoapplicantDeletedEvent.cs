using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantApplicationRequestCoapplicantDeletedEvent")]
    public class TenantApplicationRequestCoapplicantDeletedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestCoapplicantDeletedEvent(Guid id) => Id = id;
        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantApplicationRequestCoapplicantDeletedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantApplicationRequestCoapplicantDeletedInfo
        {
            private Guid id;

            public TenantApplicationRequestCoapplicantDeletedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
