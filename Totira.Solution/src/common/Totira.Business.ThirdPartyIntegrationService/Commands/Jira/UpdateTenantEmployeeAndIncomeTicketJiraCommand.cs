using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Commands.Jira
{
    [RoutingKey("UpdateTenantEmployeeAndIncomeTicketJiraCommand")]
    public class UpdateTenantEmployeeAndIncomeTicketJiraCommand : ICommand
    {
        public long? timestamp { get; set; }
        public string? webhookEvent { get; set; }
        public string? Issue_event_type_name { get; set; }
        public User? User { get; set; }
        public Comment? Comment { get; set; }
        public Issue? Issue { get; set; }
        public Changelog? changelog { get; set; }
    }
}