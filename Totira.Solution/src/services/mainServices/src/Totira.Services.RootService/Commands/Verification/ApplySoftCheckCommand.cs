using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.ThirdPartyIntegration.Certn.Model.PropertyManagement;

namespace Totira.Services.RootService.Commands.Verification;

[RoutingKey("ApplySoftCheckCommand")]
public record ApplySoftCheckCommand(Guid TenantId, SoftCheckRequestModel Request) : ICommand;
