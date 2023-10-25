using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries;

public record QueryTenantEmployeeIncomesById(Guid TenantId) : IQuery;