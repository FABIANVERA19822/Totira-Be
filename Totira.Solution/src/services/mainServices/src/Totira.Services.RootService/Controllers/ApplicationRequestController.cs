using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Totira.Services.RootService.Commands;
using Totira.Services.RootService.DTO;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Controller;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;

namespace Totira.Services.RootService.Controllers
{
    public class ApplicationRequestController : DefaultBaseController
    {
        private readonly IContextFactory _contextFactory;
        private readonly IQueryRestClient _queryRestClient;
        private readonly IEventBus _bus;
        private readonly RestClientOptions _restClientOptions;
        private readonly ILogger<OutsideController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationRequestController(IQueryRestClient queryRestClient,
           IEventBus bus,
           IOptions<RestClientOptions> restClientOptions,
           IContextFactory contextFactory,
           ILogger<OutsideController> logger,
           IHttpContextAccessor httpContextAccessor)
        {
            _contextFactory = contextFactory;
            _queryRestClient = queryRestClient;
            _bus = bus;
            _restClientOptions = restClientOptions.Value;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Return last Application Request
        /// </summary>
        /// <param name="tenantId">owner of the application request</param>
        /// <returns>ApplicationRequestDto </returns>
        [Authorize(Policy = "AppOptions")]
        [HttpGet("{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantApplicationRequestDto>> GetLastApplicationRequest(Guid tenantId)
        {
            var applicationRequest = await _queryRestClient.GetAsync<GetTenantApplicationRequestDto>($"{_restClientOptions.User}/ApplicationRequest/{tenantId}");
            if (applicationRequest == null)
            {
                return NotFound();
            }
            else
            {
                return applicationRequest;
            }
        }

        /// <summary>
        /// Return Application Request by id
        /// </summary>
        /// <param name="tenantId">owner of the application request</param>
        /// <returns>ApplicationRequestDto </returns>
        [Authorize(Policy = "AppOptions")]
        [HttpGet("{tenantId}/{applicationRequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantApplicationRequestDto>> GetApplicationRequestById(Guid tenantId, Guid applicationRequestId)
        {
            var applicationRequest = await _queryRestClient.GetAsync<GetTenantApplicationRequestDto>($"{_restClientOptions.User}/ApplicationRequest/{tenantId}/{applicationRequestId}");
            if (applicationRequest == null)
            {
                return NotFound();
            }
            else
            {
                return applicationRequest;
            }
        }

        /// <summary>
        /// Return all applications Request
        /// </summary>
        /// <param name="tenantId">Owner of applications</param>
        /// <returns>list of application request during the time</returns>
        [Authorize(Policy = "AppOptions")]
        [HttpGet("all/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAllTenantApplicationRequestDto>> GetAllApplicationDetails(Guid tenantId)
        {
            var applicationsRequest = await _queryRestClient.GetAsync<GetAllTenantApplicationRequestDto>($"{_restClientOptions.User}/ApplicationRequest/All/{tenantId}");
            if (applicationsRequest == null)
            {
                return NotFound();
            }
            else
            {
                return applicationsRequest;
            }
        }

        /// <summary>
        ///Create an application Request, Tenant Main Applicant with ApplicationDetails
        /// The Application is prepared for add cosigners with Group Application or create a Single Application
        /// </summary>
        /// <param name="command"></param>
        /// <returns>200 accepted</returns>
        [Authorize(Policy = "AppOptions")]
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateApplicationRequest(CreateTenantApplicationRequestCommand command)
        {
            string url = Url.Action("Get", "ApplicationRequest", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Allow add coapplicants and guarantor and send invitation by email
        /// Group application profile invitation
        /// </summary>
        /// <param name="command"></param>
        /// <returns>200 accepted</returns>
        [Authorize(Policy = "AppOptions")]
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCoapplicantToApplicationRequest(UpdateTenantApplicationRequestCommand command)
        {
            string url = Url.Action("Get", "ApplicationRequest", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Allow remove coapplicant for application request when coapplicant has an account
        /// <param name="applicationRequestId"> application request</param>
        /// <param name="tenantId"> tenant main application id</param>
        /// <param name="coapplicantId"> id of coapplicant with an account</param>
        /// <returns>200 accepted</returns>
        [Authorize(Policy = "AppOptions")]
        [HttpDelete("{tenantId}/{applicationRequestId}/applicants/{coapplicantId}/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPropertyDto>> RemoveCoapplicantToApplicationRequest(Guid applicationRequestId, Guid tenantId, Guid coapplicantId)
        {
            DeleteTenantApplicationRequestCoapplicantCommand command = new DeleteTenantApplicationRequestCoapplicantCommand { TenantId = tenantId, ApplicationRequestId = applicationRequestId, CoapplicantId = coapplicantId };
            string url = Url.Action("Get", "ApplicationRequest", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Allow remove guarantor for application request when guarantor has an account
        /// <param name="applicationRequestId"> application request</param>
        /// <param name="tenantId"> tenant main application id</param>
        /// <param name="guarantorId"> id of guarantor with an account</param>
        /// <returns>200 accepted</returns>
        [HttpDelete("{tenantId}/{applicationRequestId}/guarantor/{guarantorId}/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<IActionResult> RemoveGuarantorToApplicationRequest(Guid applicationRequestId, Guid tenantId, Guid guarantorId)
        {
            DeleteTenantApplicationRequestGuarantorCommand command = new DeleteTenantApplicationRequestGuarantorCommand { TenantId = tenantId, ApplicationRequestId = applicationRequestId, GuarantorId = guarantorId };
            string url = Url.Action("Get", "ApplicationRequest", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Allow remove application request
        /// <param name="applicationRequestId"> application request</param>
        /// <param name="tenantId"> tenant main application id</param>
        /// <returns>200 accepted</returns>
        [HttpDelete("{tenantId}/{applicationRequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<IActionResult> RemoveApplicationRequest(Guid applicationRequestId, Guid tenantId)
        {
            DeleteTenantApplicationRequestCommand command = new DeleteTenantApplicationRequestCommand { TenantId = tenantId, ApplicationRequestId = applicationRequestId };

            string url = Url.Action("Get", "ApplicationRequest", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Return values of applicationRequest associated to invitation to join
        /// </summary>
        /// <param name="invitationId">InvitationId that have send to cosigners from mainApplicant</param>
        /// <returns>list of application request during the time</returns>
        [HttpGet("InvitationCoapplicants/{invitationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<ActionResult<GetApplicationRequestbyInvitationDto>> GetApplicationRequestbyInvitationId(Guid invitationId)
        {
            var applicationRequest = await _queryRestClient.GetAsync<GetApplicationRequestbyInvitationDto>($"{_restClientOptions.User}/ApplicationRequest/linkverification/{invitationId}");
            if (applicationRequest == null)
            {
                return NotFound();
            }
            else
            {
                return applicationRequest;
            }
        }

        /// <summary>
        /// Return all invitations associated with applicationRequestId
        /// </summary>
        /// <param name="applicationRequestId">applicationRequest associated to tenantMainApplicant</param>
        /// <returns>list of invitations to join during the time</returns>
        [HttpGet("InvitationCoapplicants/All/{applicationRequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<ActionResult<GetAllInvitationsToJoinByApplicationRequestDto>> GetAllInvitationstoJoinByApplicationRequestId(Guid applicationRequestId)
        {
            var applicationRequest = await _queryRestClient.GetAsync<GetAllInvitationsToJoinByApplicationRequestDto>($"{_restClientOptions.User}/ApplicationRequest/InvitationCoapplicants/All/{applicationRequestId}");
            if (applicationRequest == null)
            {
                return NotFound();
            }
            else
            {
                return applicationRequest;
            }
        }

        /// <summary>
        /// Allow update invitation application request link to coapplicants
        /// isActive has to be false when tenant want to expired link invitation
        /// </summary>
        /// <param name="command"></param>
        /// <returns>200 accepted</returns>
        [HttpPut("InvitationCoapplicants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<IActionResult> UpdateApplicationRequestInvitationCoapplicants(UpdateApplicationRequestInvitationCommand command)
        {
            string url = Url.Action("Get", "ApplicationRequest", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Return tenant role within a specific Request by id
        /// </summary>
        /// <param name="tenantId">member of the application request</param>
        /// <param name="applicationId">member of the application request</param>
        /// <returns>string </returns>
        [HttpGet("requestrole/{tenantId}/{applicationDetailsId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<ActionResult<string>> GetTenantApplicationRole(Guid tenantId, Guid applicationDetailsId)
        {
            var role = await _queryRestClient.GetAsync<string>($"{_restClientOptions.User}/ApplicationRequest/requestrole/{tenantId}/{applicationDetailsId}");

            if (role == null)
                return NotFound();
            return role;
        }

        /// <summary>
        /// Return if every memeber of a request accepted terms and conditions
        /// </summary>
        /// <param name="applicationRequestId">id of the application request</param>
        /// <returns> boolean </returns>
        [HttpGet("requestTermsAndConds/{applicationRequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<ActionResult<bool?>> GetRequestTermsAndConditions(Guid applicationRequestId)
        {
            var termsAndConds = await _queryRestClient.GetAsync<bool?>($"{_restClientOptions.User}/ApplicationRequest/requestTermsAndConds/{applicationRequestId}");

            if (termsAndConds == null)
                return NotFound();
            return termsAndConds;
        }

        /// <summary>
        /// Return Shared Profile Application Request
        /// </summary>
        /// <param name="command"></param>
        /// <returns>202 accepted</returns>
        [HttpPost("ShareProfileApplication")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<IActionResult> SaveTenantShareProfile([FromBody] CreateTenantShareProfileCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "ShareProfileApplication", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Update Application Request Share profile terms and conditions
        /// </summary>
        /// <param name="command"></param>
        /// <returns>200 accepted</returns>
        [HttpPut("UpdateTenantShareProfileTermsConditions")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateTenantShareProfileTermsConditions([FromBody] UpdateTenantShareProfileTermsCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null && Guid.TryParse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value, out Guid userIdAux) ? userIdAux : command.Id;
            string url = Url.Action("Get", "UpdateTenantShareProfileTermsConditions", new { id = command.Id, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Return validation for access code and email verification Multi Tenant
        /// </summary>
        /// <param name="tenantId">member of the application request</param>
        /// <param name="accessCode">access code generated</param>
        /// <param name="email">email for verification</param>
        /// <returns>string </returns>
        [HttpGet("CheckTenantShareProfileApplication/{tenantId}/{accessCode}/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<ActionResult<GetTenantShareProfileForCheckCodeAndEmailDto>> GetTenantShareProfileApplication(Guid tenantId, int accessCode, string email)
        {
            var validation = await _queryRestClient.GetAsync<GetTenantShareProfileForCheckCodeAndEmailDto>($"{_restClientOptions.User}/user/CheckTenantShareProfileForCheckCode/{tenantId}/{accessCode}/{email}");
            if (validation == null)
            {
                return NotFound();
            }
            else
            {
                return validation;
            }
        }

        /// <summary>
        /// Method to save application type when tenant create they profile and choose type between single aplication/group application
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("ApplicationType")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<IActionResult> SaveApplicationType([FromBody] CreateTenantApplicationTypeCommand command)
        {
            string url = Url.Action("Get", "ApplicationType", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Return applicationType by tenantId
        /// </summary>
        /// <param name="tenantId">Owner of application</param>
        /// <returns>Return applicationType</returns>
        [HttpGet("ApplicationType/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<ActionResult<GetTenantApplicationTypeByDto>> GetTenantApplicationTypeByTenantId(Guid tenantId)
        {
            var status = await _queryRestClient.GetAsync<GetTenantApplicationTypeByDto>($"{_restClientOptions.User}/user/GetTenantApplicationTypeByTenantId/{tenantId}");
            if (status == null)
            {
                return NotFound();
            }
            else
            {
                return status;
            }
        }

        /// <summary>
        /// Update applicationType could be single aplication o group application
        /// </summary>
        /// <param name="command"></param>
        [HttpPut("ApplicationType")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<IActionResult> UpdateTenantApplicationType([FromBody] UpdateTenantApplicationTypeCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "ApplicationType", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Allow add coapplicants and guarantor and send invitation by email
        /// Group application profile invitation
        /// </summary>
        /// <param name="command"></param>
        /// <returns>200 accepted</returns>
        [HttpPut("ApplicationRequestInvitationResponse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<IActionResult> ApplicationRequestInvitationResponse(ApplicationRequestInvitationResponseCommand command)
        {
            string url = Url.Action("Get", "ApplicationRequestInvitationResponse", new { id = command.InvitationId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.InvitationId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Method to share profile of group application tenant verified via email
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("GroupApplicationShareProfile")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<IActionResult> SaveGroupApplicationShareProfile([FromBody] CreateGroupApplicationShareProfileCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.ApplicationId;
            string url = Url.Action("Get", "GroupApplicationShareProfile", new { id = command.ApplicationId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Return validation for access code and email verification Multi Tenant
        /// </summary>
        /// <param name="tenantId">member of the application request</param>
        /// <param name="accessCode">access code generated</param>
        /// <param name="email">email for verification</param>
        /// <returns>string </returns>
        [HttpGet("CheckTenantGroupShareProfileApplication/{tenantId}/{accessCode}/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<ActionResult<GetTenantShareProfileForCheckCodeAndEmailDto>> GetTenantGroupShareProfileApplication(Guid tenantId, int accessCode, string email)
        {
            var validation = await _queryRestClient.GetAsync<GetTenantShareProfileForCheckCodeAndEmailDto>($"{_restClientOptions.User}/user/CheckTenantGroupShareProfileForCheckCode/{tenantId}/{accessCode}/{email}");
            if (validation == null)
            {
                return NotFound();
            }
            else
            {
                return validation;
            }
        }

        [HttpPut("RemoveTenantUnacceptedApplicationRequests")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<IActionResult> RemoveTenantUnacceptedApplicationRequests([FromBody] DeleteTenantUnacceptedApplicationRequestCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.RequestId;
            string url = Url.Action("Get", "RemoveTenantUnacceptedApplicationRequests", new { id = command.RequestId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }
    }
}