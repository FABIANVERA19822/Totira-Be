using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.ThirdPartyIntegration.Certn.Model.PropertyManagement;

namespace Totira.Services.RootService.Commands.Verification;

[RoutingKey("ApplyCreditCheckCommand")]
public record ApplyCreditCheckCommand(Guid TenantId, CreditCheckRequesetModel Request) : ICommand;
