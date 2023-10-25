using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;
using static Totira.Business.ThirdPartyIntegrationService.Queries.VerifiedProfile.QueryVerifiedProfile;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Services.ThirdPartyIntegrationService.Controllers
{
    public class VerifiedProfileController : DefaultBaseController
    {
        private readonly IQueryHandler<QueryVerifiedProfileByTenantId, GetTenantVerifiedProfileDto> _verifiedDataHandler;
        private readonly IQueryHandler<QueryEmailConfirmationByTenantId, GetTenantVerifiedProfileDto> _emailConfirmationHandler;
        private readonly IQueryHandler<QueryVerifiedProfile, ListTenantVerifiedProfileDto> _profilesHandler;
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfileRepository;
        private readonly ILogger<VerifiedProfileController> _logger;

        public VerifiedProfileController(IQueryHandler<QueryVerifiedProfileByTenantId, GetTenantVerifiedProfileDto> verifiedDataHandler,
            IQueryHandler<QueryEmailConfirmationByTenantId, GetTenantVerifiedProfileDto> emailConfirmationHandler,
            IQueryHandler<QueryVerifiedProfile, ListTenantVerifiedProfileDto> profilesHandler,
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository,
        ILogger<VerifiedProfileController> logger)
        {
            _verifiedDataHandler = verifiedDataHandler;
            _emailConfirmationHandler = emailConfirmationHandler;
            _profilesHandler = profilesHandler;
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;
            _logger = logger;
        }

        [HttpGet("EmailConfirmation/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<GetTenantVerifiedProfileDto>>GetEmailConfirmation(string tenantId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Error, invalid request");
                return BadRequest();
            }
            Expression<Func<TenantVerifiedProfile, bool>> expression = (p => p.TenantId == Guid.Parse(tenantId));
            var info = (await _tenantVerifiedProfileRepository.Get(expression)).FirstOrDefault();

            if (info == null)
            {
                _logger.LogError("Tenant has not a Verified profile validation process.");
                return Conflict($"Tenant {tenantId} has not a Verified profile validation process.");
            }

            return await _emailConfirmationHandler.HandleAsync(new QueryEmailConfirmationByTenantId(tenantId));
        }

        [HttpGet("profiles/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<GetTenantVerifiedProfileDto>> GetProfile(string tenantId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Error, invalid request");
                return BadRequest();
            }
            Expression<Func<TenantVerifiedProfile, bool>> expression = (p => p.TenantId == Guid.Parse(tenantId));
            var info = (await _tenantVerifiedProfileRepository.Get(expression)).FirstOrDefault();

            if (info == null)
            {
                _logger.LogError("Tenant has not a Verified profile validation process.");
                return Conflict($"Tenant {tenantId} has not a Verified profile validation process.");
            }

            return await _verifiedDataHandler.HandleAsync(new QueryVerifiedProfileByTenantId(tenantId));
        }

        [HttpGet("profiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListTenantVerifiedProfileDto>> GetAllProfiles()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Error, invalid request");
                return BadRequest();
            }
            return await _profilesHandler.HandleAsync(new QueryVerifiedProfile(EnumVerifiedProfileSortBy.CreatedOn, 1, 20));
        }
    }
}
