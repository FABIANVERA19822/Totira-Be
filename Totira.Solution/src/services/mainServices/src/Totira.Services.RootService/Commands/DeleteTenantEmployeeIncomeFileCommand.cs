using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands;

[RoutingKey("DeleteTenantEmployeeIncomeFileCommand")]
public record DeleteTenantEmployeeIncomeFileCommand(Guid TenantId, Guid IncomeId, string FileName) : ICommand;
