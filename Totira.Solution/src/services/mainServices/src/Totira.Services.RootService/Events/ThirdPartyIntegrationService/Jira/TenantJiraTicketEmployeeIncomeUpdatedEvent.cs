using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.ThirdPartyIntegrationService.Jira
{ 

    [RoutingKey("TenantJiraTicketEmployeeIncomeUpdatedEvent")]
    public class TenantJiraTicketEmployeeIncomeUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantJiraTicketEmployeeIncomeUpdatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantJiraTicketEmployeeIncomeUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantJiraTicketEmployeeIncomeUpdatedInfo
        {
            private Guid id;

            public TenantJiraTicketEmployeeIncomeUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
