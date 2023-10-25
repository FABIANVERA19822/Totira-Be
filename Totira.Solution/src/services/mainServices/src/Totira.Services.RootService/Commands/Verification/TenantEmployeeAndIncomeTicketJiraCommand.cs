using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands.Verification
{
    [RoutingKey("TenantEmployeeAndIncomeTicketJiraCommand")]
    public class TenantEmployeeAndIncomeTicketJiraCommand : ICommand
    {
        public Guid TenantId { get; set; }

    }
}
