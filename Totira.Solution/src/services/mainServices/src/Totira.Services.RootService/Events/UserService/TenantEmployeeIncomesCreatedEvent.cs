using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantEmployeeIncomesCreatedEvent")]
    public class TenantEmployeeIncomesCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantEmployeeIncomesCreatedEvent(Guid id)
        {
            Id = id;
        }

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantEmployeeIncomesCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantEmployeeIncomesCreatedInfo
        {
            private Guid id;

            public TenantEmployeeIncomesCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
