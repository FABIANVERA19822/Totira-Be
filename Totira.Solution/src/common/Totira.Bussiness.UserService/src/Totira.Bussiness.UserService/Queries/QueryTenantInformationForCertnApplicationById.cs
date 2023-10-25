using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries;

public record QueryTenantInformationForCertnApplicationById(Guid TenantId) : IQuery;
