
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands;

[RoutingKey(nameof(DeleteTenantStudentDetailCommand))]
public record DeleteTenantStudentDetailCommand(Guid TenantId, Guid StudyId) : ICommand;