using Microsoft.AspNetCore.Mvc;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;

namespace Totira.Services.UserService.Controllers
{
    public class ApplicationRequestController : DefaultBaseController
    {

        private readonly IQueryHandler<QueryApplicationRequestByTenantId, GetTenantApplicationRequestDto> _getTenantApplicationRequestHandler;
        private readonly IQueryHandler<QueryAllApplicationRequestByTenantId, GetAllTenantApplicationRequestDto> _getAllTenantApplicationRequestHandler;
        private readonly ILogger<ReferralInfoController> _logger;
        public ApplicationRequestController(
            IQueryHandler<QueryApplicationRequestByTenantId, GetTenantApplicationRequestDto> getTenantApplicationRequestHandler,
            IQueryHandler<QueryAllApplicationRequestByTenantId, GetAllTenantApplicationRequestDto> getAllTenantApplicationRequestHandler,
            ILogger<ReferralInfoController> logger)
        {
            _getTenantApplicationRequestHandler = getTenantApplicationRequestHandler;
            _getAllTenantApplicationRequestHandler = getAllTenantApplicationRequestHandler;
            _logger = logger;
        }

        /// <summary>
        /// Return last Application Request
        /// </summary>
        /// <param name="tenantId">user part of the application request</param>
        /// <returns>ApplicationRequestDto </returns>
        [HttpGet("{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantApplicationRequestDto>> GetLastApplicationRequest(Guid tenantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantApplicationRequestHandler.HandleAsync(new QueryApplicationRequestByTenantId(tenantId));

        }

        /// <summary>
        /// Return Application Request by id
        /// </summary>
        /// <param name="tenantId">user part of the application request</param>
        /// <param name="applicationId">owner of the application request</param>
        /// <returns>ApplicationRequestDto </returns>
        [HttpGet("{tenantId}/{applicationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantApplicationRequestDto>> GetApplicationRequestById(Guid tenantId, Guid applicationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantApplicationRequestHandler.HandleAsync(new QueryApplicationRequestByTenantId(tenantId, applicationId));

        }

        /// <summary>
        /// Return all applications Request
        /// </summary>
        /// <param name="tenantId">Owner of applications</param>
        /// <returns>list of application request during the time</returns>
        [HttpGet("all/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAllTenantApplicationRequestDto>> GetAllApplicationDetails(Guid tenantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getAllTenantApplicationRequestHandler.HandleAsync(new QueryAllApplicationRequestByTenantId(tenantId));
        }
    }
}
