using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO.GroupTenant;
using Totira.Bussiness.UserService.DTO.ThirdpartyService;
using Totira.Bussiness.UserService.Extensions.DomainExtensions;
using Totira.Bussiness.UserService.Queries.Group;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries;

public class GetTenantGroupVerifiedProfileQueryHandler : IQueryHandler<QueryGetGroupVerifiedProfilebyTenantId, GetTenantGroupVerifiedProfileDto>
{
    private readonly ILogger<GetTenantGroupVerifiedProfileQueryHandler> _logger;
    private readonly IRepository<TenantGroupVerificationProfile, Guid> _tenantGroupVerifiedProfileRepository;
    private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
    private readonly IQueryRestClient _queryRestClient;
    private readonly RestClientOptions _restClientOptions;

    public GetTenantGroupVerifiedProfileQueryHandler(
        ILogger<GetTenantGroupVerifiedProfileQueryHandler> logger,
        IRepository<TenantGroupVerificationProfile, Guid> tenantGroupVerifiedProfileRepository,
        IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
        IQueryRestClient queryRestClient,
        IOptions<RestClientOptions> restClientOptions)
    {
        _logger = logger;
        _tenantGroupVerifiedProfileRepository = tenantGroupVerifiedProfileRepository;
        _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
        _queryRestClient = queryRestClient;
        _restClientOptions = restClientOptions.Value;
    }

    public async Task<GetTenantGroupVerifiedProfileDto> HandleAsync(QueryGetGroupVerifiedProfilebyTenantId query)
    {
        var result = new GetTenantGroupVerifiedProfileDto();

        var tenantGroupVerifiedProfile = await _tenantGroupVerifiedProfileRepository.GetByIdAsync(query.TenantId);
        if (tenantGroupVerifiedProfile is not null)
        {
            if (tenantGroupVerifiedProfile.IsUnderInitialRevision || tenantGroupVerifiedProfile.IsUnderNewRevision)
            {
                var tenantApplicationRequest = await _tenantApplicationRequestRepository.LastOrDefault(x => x.TenantId == query.TenantId);
                if (tenantApplicationRequest is null)
                    return result;

                var tenantsIds = new List<Guid>();

                if (tenantApplicationRequest.Coapplicants is not null && tenantApplicationRequest.Coapplicants.Any())
                {
                    // Coapplicants
                    tenantsIds.AddRange(tenantApplicationRequest.Coapplicants
                        .Where(x => x.Id.HasValue)
                        .Select(x => x.Id!.Value)
                        .ToList());
                }

                // Main tenant
                tenantsIds.Add(tenantApplicationRequest.TenantId);

                // Guarantor
                if (tenantApplicationRequest.Guarantor is not null && tenantApplicationRequest.Guarantor.Id.HasValue)
                    tenantsIds.Add(tenantApplicationRequest.Guarantor.Id.Value);

                var validTenants = tenantsIds
                    .Select(x => new VerificationResult
                    {
                        TenantId = x,
                        IsValid = false
                    })
                    .ToList();

                foreach (var tenant in validTenants)
                {
                    var url = $"{_restClientOptions.ThirdPartyIntegration}/VerifiedProfile/profiles/{tenant.TenantId}";
                    var response = await _queryRestClient.GetAsync<GetTenantVerifiedProfileDto>(url);
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        tenant.IsValid = response.Content.Certn && response.Content.Jira && response.Content.Persona;
                    }
                }

                if (validTenants.All(x => x.IsValid))
                {
                    if (tenantGroupVerifiedProfile.IsReVerificationEnabled)
                        tenantGroupVerifiedProfile.CompleteReVerification();
                    else
                        tenantGroupVerifiedProfile.CompleteInitialVerification();

                    await _tenantGroupVerifiedProfileRepository.Update(tenantGroupVerifiedProfile);
                }
            }

            result.IsGroupUnderRevision = tenantGroupVerifiedProfile.IsUnderInitialRevision || tenantGroupVerifiedProfile.IsUnderNewRevision;
            result.IsGroupVerificationComplete = tenantGroupVerifiedProfile.IsInitialVerificationComplete ||
                (tenantGroupVerifiedProfile.IsReVerificationEnabled && tenantGroupVerifiedProfile.IsLastVerificationComplete);
        }
        
        return result;
    }
}

class VerificationResult
{
    public Guid TenantId { get; set; }
    public bool IsValid { get; set; }
}