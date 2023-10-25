using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Commands.Jira;

[RoutingKey(nameof(ApplyJiraGroupCommand))]
public class ApplyJiraGroupCommand : ICommand
{
    public List<Guid> Tenants { get; set; } = new();
}