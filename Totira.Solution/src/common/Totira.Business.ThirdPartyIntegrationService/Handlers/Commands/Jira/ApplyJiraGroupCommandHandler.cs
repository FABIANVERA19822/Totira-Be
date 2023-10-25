using Microsoft.Extensions.Logging;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using static Totira.Support.Application.Messages.IMessageHandler;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands;

public class ApplyJiraGroupCommandHandler : IMessageHandler<ApplyJiraGroupCommand>
{
    private readonly ILogger<ApplyJiraGroupCommandHandler> _logger;
    private readonly IEventBus _bus;

    public ApplyJiraGroupCommandHandler(ILogger<ApplyJiraGroupCommandHandler> logger, IEventBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public async Task HandleAsync(IContext context, ApplyJiraGroupCommand command)
    {
        foreach (var tenantId in command.Tenants)
        {
            var jiraCommand = new TenantEmployeeAndIncomeTicketJiraCommand() { TenantId = tenantId };
            await _bus.PublishAsync(context, jiraCommand);
            _logger.LogInformation("Main tenant Id: {tenantId} sent to Jira.", tenantId);
        }
    }
}