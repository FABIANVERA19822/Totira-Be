using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries;

public record QueryTermsAndConditionsByTenantId(Guid TenantId) : IQuery;
