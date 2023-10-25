
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.VerifiedProfile
{
    public class QueryVerifiedProfileByTenantIdHandler : IQueryHandler<QueryVerifiedProfileByTenantId, GetTenantVerifiedProfileDto>
    {
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfileRepository;
        private readonly ILogger<QueryVerifiedProfileByTenantIdHandler> _logger;

        public QueryVerifiedProfileByTenantIdHandler (
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository,
            ILogger<QueryVerifiedProfileByTenantIdHandler > logger )
        {
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;
            _logger = logger;
        }
        public async Task<GetTenantVerifiedProfileDto> HandleAsync(QueryVerifiedProfileByTenantId query)
        {
            var TenantGuid = Guid.Parse( query.TenantId );
            Expression<Func<TenantVerifiedProfile, bool>> expression = (p => p.TenantId == TenantGuid);

            var info = (await _tenantVerifiedProfileRepository.Get(expression)).FirstOrDefault();
            
            GetTenantVerifiedProfileDto getTenantVerifieProfiledDto = new GetTenantVerifiedProfileDto();
            if (info == null)
            {
                _logger.LogWarning("Tenant has not a Verified Profile validation process.");
                return getTenantVerifieProfiledDto;
            }
            getTenantVerifieProfiledDto.TenantId = query.TenantId;
            getTenantVerifieProfiledDto.Jira = info.Jira;
            getTenantVerifieProfiledDto.Certn = info.Certn;
            getTenantVerifieProfiledDto.Persona = info.Persona;
            getTenantVerifieProfiledDto.IsVerifiedEmailConfirmation = info.IsVerifiedEmailConfirmation;

            return getTenantVerifieProfiledDto;
        }
    }
}