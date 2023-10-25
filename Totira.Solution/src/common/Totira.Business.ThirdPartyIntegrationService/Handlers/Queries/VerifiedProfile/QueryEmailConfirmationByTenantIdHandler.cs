
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.VerifiedProfile
{
    public class QueryEmailConfirmationByTenantIdHandler : IQueryHandler<QueryEmailConfirmationByTenantId, GetTenantVerifiedProfileDto>
    {
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantEmailConfirmationRepository;
        private readonly ILogger<QueryEmailConfirmationByTenantIdHandler> _logger;
        
        public QueryEmailConfirmationByTenantIdHandler(IRepository<TenantVerifiedProfile, Guid> tenantEmailConfirmationRepository,
            ILogger<QueryEmailConfirmationByTenantIdHandler> logger)
        {
            _tenantEmailConfirmationRepository = tenantEmailConfirmationRepository;
            _logger = logger;
        }

        public async Task<GetTenantVerifiedProfileDto> HandleAsync(QueryEmailConfirmationByTenantId query)
        {
            var TenantGuid = Guid.Parse(query.TenantId);
            Expression<Func<TenantVerifiedProfile, bool>> expression = (p => p.TenantId == TenantGuid);

            var info = (await _tenantEmailConfirmationRepository.Get(expression)).FirstOrDefault();

            GetTenantVerifiedProfileDto getTenantVerifieProfiledDto = null;
            if (info == null)
            {
                _logger.LogWarning("Tenant has not a Verified Profile validation process.");
                return getTenantVerifieProfiledDto;
            }
            // Update object
            info.TenantId = TenantGuid;
            info.IsVerifiedEmailConfirmation = true;
            info.UpdatedOn = DateTime.UtcNow;
            await _tenantEmailConfirmationRepository.Update(info);

            return getTenantVerifieProfiledDto;
        }
    }
}
