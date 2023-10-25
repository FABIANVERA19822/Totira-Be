using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantEmployeeIncomesUpdatedEvent")]
    public class TenantEmployeeIncomesUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public string Message { get; }

        public TenantEmployeeIncomesUpdatedEvent(Guid id, string message)
        {
            Id = id;
            this.Message = message;
        }

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantEmployeeIncomesUpdatedInfo(this.Id, this.Message);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantEmployeeIncomesUpdatedInfo
        {
            private Guid Id { get; set; }
            public string Message { get; }

            public TenantEmployeeIncomesUpdatedInfo(Guid id, string message)
            {
                this.Id = id;
                this.Message = message;
            }
        }
    }
}
