using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands;

[RoutingKey(nameof(DeleteTenantStudentFinancialDetailFileCommand))]
public record DeleteTenantStudentFinancialDetailFileCommand(
    Guid TenantId,
    Guid StudyId,
    string FileName) : ICommand;
