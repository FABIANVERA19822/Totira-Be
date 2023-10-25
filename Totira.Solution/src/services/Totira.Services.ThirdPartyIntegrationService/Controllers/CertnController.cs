using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Domain.Certn;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries.Certn;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;
using static Totira.Business.ThirdPartyIntegrationService.Queries.Certn.QueryApplication;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Services.ThirdPartyIntegrationService.Controllers
{
    public class CertnController : DefaultBaseController
    {
        private readonly IQueryHandler<QueryApplicationByTenantId, GetCertnApplicationDto> _certndataHandler;
        private readonly IQueryHandler<QueryApplication, ListTenantApplicationDto> _applicantsHandler;
        private readonly IRepository<TenantApplications, string> _tenantApplicationsRepository;
        private readonly ILogger<CertnController> _logger;

        public CertnController (
            IQueryHandler<QueryApplicationByTenantId, GetCertnApplicationDto> certndataHandler,
            IQueryHandler<QueryApplication, ListTenantApplicationDto> applicantsHandler,
            IRepository<TenantApplications, string> tenantApplicationsRepository,
            ILogger<CertnController> logger
            )
        {
            _certndataHandler = certndataHandler;
            _applicantsHandler = applicantsHandler;
            _tenantApplicationsRepository = tenantApplicationsRepository;
            _logger = logger;
        }


        [HttpGet("applicants/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<GetCertnApplicationDto>> GetApplication(string tenantId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Error, invalid request");
                return BadRequest();
            }
            Expression<Func<TenantApplications, bool>> expression = (p => p.Id == tenantId);
            var info = (await _tenantApplicationsRepository.Get(expression)).FirstOrDefault();

            var result1 =
                info != null ?
                    new TenantApplications(info.Id, info.Applications, info.CreatedOn) :
                    new TenantApplications();


            if (result1.Applications.Count == 0)
            {
                _logger.LogError("Tenant has not a Certn validation process.");
                return Conflict($"Tenant {tenantId} has not a Certn validation process.");
            }

            return await _certndataHandler.HandleAsync(new QueryApplicationByTenantId(tenantId));
        }


        [HttpGet("applicants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListTenantApplicationDto>> GetAllApplicants()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Error, invalid request");
                return BadRequest();
            }
            return await _applicantsHandler.HandleAsync(new QueryApplication(EnumApplicantSortBy.CreatedOn, 1, 20));
        }
    }
}
