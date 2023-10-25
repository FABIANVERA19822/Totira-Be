using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Commands.Jira
{
    [RoutingKey("TenantEmployeeAndIncomeTicketJiraCommand")]
    public class TenantEmployeeAndIncomeTicketJiraCommand : ICommand
    {
        public Guid TenantId { get; set; }

    }
}
