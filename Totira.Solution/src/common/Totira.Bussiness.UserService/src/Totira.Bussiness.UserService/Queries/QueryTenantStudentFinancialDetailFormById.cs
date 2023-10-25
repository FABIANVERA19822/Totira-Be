using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries;

public record QueryTenantStudentFinancialDetailFormById(Guid TenantId) : IQuery;
