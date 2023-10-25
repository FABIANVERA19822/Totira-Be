using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Events.Jira
{ 

    [RoutingKey("TenantJiraTicketEmployeeIncomeUpdatedEvent")]
    public class TenantJiraTicketEmployeeIncomeUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantJiraTicketEmployeeIncomeUpdatedEvent(Guid id) => Id = id;

    }
}
