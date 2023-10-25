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
        private readonly IQueryHandler<QueryApplicationRequestbyInvitationId, GetApplicationRequestbyInvitationDto> _getApplicationRequestValidationCoapplicantByIdHandler;
        private readonly IQueryHandler<QueryInvitationsByApplicationRequestById, GetAllInvitationsToJoinByApplicationRequestDto> _getInvitationsbyApplicationRequestByIdHandler;
        private readonly IQueryHandler<QueryTenantApplicationRoleByTenantId, string> _getTenantApplicationRoleHandler;
        private readonly IQueryHandler<QueryTermsAndConditionsByApplicationRequestId, bool?> _getApplicationRequestTermsAndConditionsHandler;
        private readonly ILogger<ReferralInfoController> _logger;
        public ApplicationRequestController(
            IQueryHandler<QueryApplicationRequestByTenantId, GetTenantApplicationRequestDto> getTenantApplicationRequestHandler,
            IQueryHandler<QueryAllApplicationRequestByTenantId, GetAllTenantApplicationRequestDto> getAllTenantApplicationRequestHandler,
            IQueryHandler<QueryApplicationRequestbyInvitationId, GetApplicationRequestbyInvitationDto> getApplicationRequestValidationCoapplicantByIdHandler,
            IQueryHandler<QueryInvitationsByApplicationRequestById, GetAllInvitationsToJoinByApplicationRequestDto> getInvitationsbyApplicationRequestByIdHandler,
            IQueryHandler<QueryTenantApplicationRoleByTenantId, string> getTenantApplicationRoleHandler,
            IQueryHandler<QueryTermsAndConditionsByApplicationRequestId, bool?> getApplicationRequestTermsAndConditionsHandler,
            ILogger<ReferralInfoController> logger)
        {
            _getTenantApplicationRequestHandler = getTenantApplicationRequestHandler;
            _getAllTenantApplicationRequestHandler = getAllTenantApplicationRequestHandler;
            _getApplicationRequestValidationCoapplicantByIdHandler = getApplicationRequestValidationCoapplicantByIdHandler;
            _getInvitationsbyApplicationRequestByIdHandler = getInvitationsbyApplicationRequestByIdHandler;
            _getTenantApplicationRoleHandler = getTenantApplicationRoleHandler;
            _getApplicationRequestTermsAndConditionsHandler = getApplicationRequestTermsAndConditionsHandler;
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

        /// <summary>
        /// Return values of applicationRequest associated to invitation to join
        /// </summary>
        /// <param name="invitationId">InvitationId that have send to cosigners from mainApplicant</param>
        /// <returns>list of application request during the time</returns>
        [HttpGet("linkverification/{invitationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetApplicationRequestbyInvitationDto>> GetApplicationRequestbyInvitationId(Guid invitationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getApplicationRequestValidationCoapplicantByIdHandler.HandleAsync(new QueryApplicationRequestbyInvitationId(invitationId));
        }

        /// <summary>
        /// Return all invitations associated with applicationRequestId
        /// </summary>
        /// <param name="applicationRequestId">applicationRequest associated to tenantMainApplicant</param>
        /// <returns>list of invitations to join during the time</returns>
        [HttpGet("InvitationCoapplicants/All/{applicationRequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAllInvitationsToJoinByApplicationRequestDto>> GetAllInvitationstoJoinByApplicationRequestId(Guid applicationRequestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getInvitationsbyApplicationRequestByIdHandler.HandleAsync(new QueryInvitationsByApplicationRequestById(applicationRequestId));
        }


        /// <summary>
        /// Return Application Request by id
        /// </summary>
        /// <param name="tenantId">user part of the application request</param>
        /// <param name="applicationDetailsId">owner of the application request</param>
        /// <returns>ApplicationRequestDto </returns>
        [HttpGet("requestrole/{tenantId}/{applicationDetailsId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetTenantApplicationRole(Guid tenantId, Guid applicationDetailsId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getTenantApplicationRoleHandler.HandleAsync(new QueryTenantApplicationRoleByTenantId(tenantId, applicationDetailsId));
        }

        /// <summary>
        /// Return if every memeber of a request accepted terms and conditions
        /// </summary>
        /// <param name="applicationRequestId">id of the application request</param>
        /// <returns> boolean </returns>
        [HttpGet("requestTermsAndConds/{applicationRequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool?>> GetRequestTermsAndConditions(Guid applicationRequestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getApplicationRequestTermsAndConditionsHandler.HandleAsync(new QueryTermsAndConditionsByApplicationRequestId(applicationRequestId));
        }
    }
}
