using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands;

[RoutingKey(nameof(DeleteTenantStudentFinancialDetailFileCommand))]
public record DeleteTenantStudentFinancialDetailFileCommand(
    Guid TenantId,
    Guid StudyId,
    string FileName) : ICommand;
