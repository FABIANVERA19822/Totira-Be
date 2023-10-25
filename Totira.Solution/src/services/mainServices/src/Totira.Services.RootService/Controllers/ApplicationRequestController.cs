using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
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


        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateApplicationRequest(CreateTenantApplicationRequestCommand command)
        {

            string url = Url.Action("Get", "ApplicationRequest", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            var context = _contextFactory.Create(url,userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

       

        /// <summary>
        /// Allow add coapplicants and guarantor
        /// </summary>
        /// <param name="command"></param>
        /// <returns>200 accepted</returns>
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


        [HttpDelete("{tenantId}/{applicationRequestId}/guarantor/{guarantorId}/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveGuarantorToApplicationRequest(Guid applicationRequestId, Guid tenantId, Guid guarantorId)
        {

            DeleteTenantApplicationRequestGuarantorCommand command = new DeleteTenantApplicationRequestGuarantorCommand { TenantId = tenantId, ApplicationRequestId = applicationRequestId, GuarantorId = guarantorId };
            string url = Url.Action("Get", "ApplicationRequest", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [HttpDelete("{tenantId}/{applicationRequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveApplicationRequest(Guid applicationRequestId,Guid tenantId)
        {
            DeleteTenantApplicationRequestCommand command = new DeleteTenantApplicationRequestCommand { TenantId = tenantId, ApplicationRequestId = applicationRequestId };

            string url = Url.Action("Get", "ApplicationRequest", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }





    }
}
