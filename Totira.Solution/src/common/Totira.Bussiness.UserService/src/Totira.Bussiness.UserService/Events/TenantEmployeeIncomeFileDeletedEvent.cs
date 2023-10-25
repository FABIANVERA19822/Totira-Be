using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events;

[RoutingKey("TenantEmployeeIncomeFileDeletedEvent")]
public record TenantEmployeeIncomeFileDeletedEvent(Guid TenantId, Guid IncomeId, string FileName) : IEvent;