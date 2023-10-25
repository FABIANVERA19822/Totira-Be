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

        public TenantEmployeeIncomeIdDeletedEvent(Guid id, Guid incomeId)
        {
            Id = id;
            IncomeId = incomeId;
        }

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantEmployeeIncomeIdDeletedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantEmployeeIncomeIdDeletedInfo
        {
            private Guid id;

            public TenantEmployeeIncomeIdDeletedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
