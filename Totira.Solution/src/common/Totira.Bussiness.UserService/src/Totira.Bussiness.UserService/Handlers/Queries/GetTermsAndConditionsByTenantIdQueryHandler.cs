using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries;

public class GetTermsAndConditionsByTenantIdQueryHandler : IQueryHandler<QueryTermsAndConditionsByTenantId, GetTermsAndConditionsByTenantIdDto>
{
    private readonly IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid> _termsAndConditionsRepository;
    private readonly ILogger<GetTermsAndConditionsByTenantIdQueryHandler> _logger;

    public GetTermsAndConditionsByTenantIdQueryHandler(
        IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid> termsAndConditionsRepository,
        ILogger<GetTermsAndConditionsByTenantIdQueryHandler> logger)
    {
        _termsAndConditionsRepository = termsAndConditionsRepository;
        _logger = logger;
    }

    public async Task<GetTermsAndConditionsByTenantIdDto> HandleAsync(QueryTermsAndConditionsByTenantId query)
    {
        var found = await _termsAndConditionsRepository.Get(x => x.TenantId == query.TenantId);

        if (!found.Any())
        {
            _logger.LogError("Tenant: {tenantId} does not exist or does not accepted terms and conditions.", query.TenantId);

            return new GetTermsAndConditionsByTenantIdDto(query.TenantId, false);
        }

        var tenant = found.First();

        return new GetTermsAndConditionsByTenantIdDto(tenant.TenantId, true);
    }
}
