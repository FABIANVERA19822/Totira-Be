using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{

    [RoutingKey("TenantEmployeeIncomeIdDeletedEvent")]
    public class TenantEmployeeIncomeIdDeletedEvent : IEvent ,INotification
    {
        public Guid Id { get; set; }
        public Guid IncomeId { get; set; }
        public string Message { get; }

        public TenantEmployeeIncomeIdDeletedEvent(Guid id, Guid incomeId, string message)
        {
            Id = id;
            IncomeId = incomeId;
            this.Message = message;
        }

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantEmployeeIncomeIdDeletedInfo(this.Id, this.Message);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantEmployeeIncomeIdDeletedInfo
        {
            private Guid id { get; set; }
            public string Message { get; }
            public TenantEmployeeIncomeIdDeletedInfo(Guid id, string message)
            {
                this.id = id;
                Message = message;
            }
        }
    }
}
