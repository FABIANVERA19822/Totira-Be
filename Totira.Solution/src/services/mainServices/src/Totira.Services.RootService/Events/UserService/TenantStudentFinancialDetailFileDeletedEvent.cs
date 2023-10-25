using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantStudentFinancialDetailFileDeletedEvent")]
    public class TenantStudentFinancialDetailFileDeletedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantStudentFinancialDetailFileDeletedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantStudentFinancialDetailFileDeletedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantStudentFinancialDetailFileDeletedInfo
        {
            private Guid id;

            public TenantStudentFinancialDetailFileDeletedInfo(Guid id)
            {
                this.id = id;
            }
        }
    }
}