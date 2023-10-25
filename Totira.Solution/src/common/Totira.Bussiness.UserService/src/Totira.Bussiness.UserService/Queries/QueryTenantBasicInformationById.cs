using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries;

public record QueryTenantBasicInformationById(Guid TenantId) : IQuery;
