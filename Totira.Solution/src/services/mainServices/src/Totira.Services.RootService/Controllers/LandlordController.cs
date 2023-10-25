using System.Net.Mime;
using System.Security.Claims;
using LanguageExt;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Totira.Services.RootService.Commands;
using Totira.Services.RootService.Commands.LandlordCommands;
using Totira.Services.RootService.DTO.Common;
using Totira.Services.RootService.DTO.Landlord.FormDtos;
using Totira.Services.RootService.DTO.Landlord.GetDtos;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Controller;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;

namespace Totira.Services.RootService.Controllers
{
    [Authorize]
    public class LandlordController : DefaultBaseController
    {
        private readonly IContextFactory _contextFactory;
        private readonly IQueryRestClient _queryRestClient;
        private readonly IEventBus _bus;
        private readonly RestClientOptions _restClientOptions;
        private readonly ILogger<LandlordController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LandlordController(
            IQueryRestClient queryRestClient,
            IEventBus bus,
            IOptions<RestClientOptions> restClientOptions,
            IContextFactory contextFactory,
            ILogger<LandlordController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _contextFactory = contextFactory;
            _queryRestClient = queryRestClient;
            _bus = bus;
            _restClientOptions = restClientOptions.Value;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }


        #region Basic Information
        [HttpGet("{id}/BasicInfo")]
        [ProducesResponseType(typeof(GetLandlordBasicInformationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, MediaTypeNames.Text.Plain)]
        public async Task<ActionResult<GetLandlordBasicInformationDto>> GetBasicInformation(Guid id)
        {
            var url = $"{_restClientOptions.User}/user/landlord/{id}/basic-info";
            var basicInfo = await _queryRestClient.GetAsync<GetLandlordBasicInformationDto>(url);

            if (basicInfo is null)
                return NotFound();

            return basicInfo;
        }

        [HttpPost("CreateLandlordBasicInfoCommand")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLandlordBasicInfo([FromBody] CreateLandlordBasicInfoCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.LandlordId;
            string url = Url.Action("Get", "UserPersonal", new { id = command.LandlordId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }
        #endregion

        #region  Landlord Identity & Contact

        [HttpPost("CreateLandlordIdentity")]
        [Consumes(typeof(FormCreateLandlordIdentityDto), "multipart/form-data")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLandlordIdentity([FromForm] FormCreateLandlordIdentityDto dto)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : dto.LandlordId;

            string url = Url.Action("Get", "CreateLandlordIdentity", new { id = dto.LandlordId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            Func<IFormFile, FileInfoDto> identityProof = file => new FileInfoDto(file);

            var command = new CreateLandlordIdentityCommand()
            {
                LandlordId = dto.LandlordId,
                PhoneNumber = new ContactInformationPhoneNumber(
                    dto.ContactInformationPhoneNumber.Number,
                    dto.ContactInformationPhoneNumber.CountryCode
                    ),
                IdentityProofs = dto.IdentityProof.Select(identityProof).ToList()
            };

            await _bus.PublishAsync(context, command);

            return Accepted(context);
        }


        [HttpGet("{id}/IdentityInfo")]
        [ProducesResponseType(typeof(GetLandlordIdentityInformationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, MediaTypeNames.Text.Plain)]
        public async Task<ActionResult<GetLandlordIdentityInformationDto>> GetIdentityInformation(Guid id)
        {
            var url = $"{_restClientOptions.User}/user/landlord/{id}/identity-info";
            var basicInfo = await _queryRestClient.GetAsync<GetLandlordIdentityInformationDto>(url);

            if (basicInfo is null)
                return NotFound();

            return basicInfo;
        }
        #endregion

        #region Claims

        [HttpPost("CreatePropertyClaimFromLandlord")]
        [Consumes(typeof(FormCreatePropertyClaimsFromLandlordDto), "multipart/form-data")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePropertyClaimFromLandlord([FromForm] FormCreatePropertyClaimsFromLandlordDto dto)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : dto.LandlordId;

            string url = Url.Action("Get", "CreatePropertyClaimFromLandlord", new { id = dto.LandlordId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            Func<IFormFile, FileInfoDto> ownershipProof = file => new FileInfoDto(file);

            Func<FormPropertyClaimDetailDto, PropertyClaimDetailDto> claims = claim =>
            new PropertyClaimDetailDto
            {
                MlsID = claim.MlsId,
                Address = claim.Address,
                Unit = claim.Unit,
                City = claim.City,
                ListingUrl = claim.ListingUrl,
                OwnershipProofs = claim.OwnershipProofs.Select(ownershipProof).ToList()
            };
            var createPropertyClaimsCommand = new CreatePropertyClaimsFromLandlordCommand()
            {
                LandlordId = dto.LandlordId,
                Role = dto.Role,
                ClaimDetails = dto.PropertyClaims.Select(claims).ToList()
            };

            await _bus.PublishAsync(context, createPropertyClaimsCommand);
            return Accepted(context);
        }

        [HttpGet("{landlordId}/PendingClaimsFromLandlord")]
        [ProducesResponseType(typeof(IEnumerable<GetPendingLandlordClaimsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GetPendingLandlordClaimsDto>>> GetPendingClaimsFromLandlord(Guid landlordId)
        {
            var url = $"{_restClientOptions.User}/user/landlord/{landlordId}/pendingClaimsFromLandlord";
            var pendingClaims = await _queryRestClient.GetAsync<IEnumerable<GetPendingLandlordClaimsDto>>(url);
            if (pendingClaims is null)
                return NotFound();

            return pendingClaims;
        }

        [HttpPost("GetLandlordClaimedProperties")]
        [ProducesResponseType(typeof(GetLandlordClaimsDisplayDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetLandlordClaimsDisplayDto>> GetLandlordClaimedProperties([FromBody] FormGetLandlordClaimsDisplayDto query)
        {
            string url = $"{_restClientOptions.User}/user/landlord/{query.LandlordId}/{query.PageNumber}/{query.PageSize}";
            var pendingClaims = await _queryRestClient.GetAsync<GetLandlordClaimsDisplayDto>(url);

            if (pendingClaims is null)
                return NotFound();

            return pendingClaims;
        }

        #endregion

        #region Application
        [HttpPost("application-request/approve")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IContext), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> ApproveApplicationRequest([FromBody]ApproveApplicationRequestCommand command)
        {
            var firstUser = _httpContextAccessor.HttpContext!.User?.FindFirst(ClaimTypes.NameIdentifier);
            var userId = firstUser is not null
                ? Guid.Parse(firstUser.Value)
                : command.TenantId;
            var url = Url.Action("Get", "Landlord", new { id = command.TenantId, version = API_VERSION}) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        
        [HttpPost("application-request/reject")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IContext), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> RejectApplicationRequest([FromBody]RejectApplicationRequestCommand command)
        {
            var firstUser = _httpContextAccessor.HttpContext!.User?.FindFirst(ClaimTypes.NameIdentifier);
            var userId = firstUser is not null
                ? Guid.Parse(firstUser.Value)
                : command.TenantId;
            var url = Url.Action("Get", "Landlord", new { id = command.TenantId, version = API_VERSION}) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("CreateLandlordPropertyClaimsJiraTicket")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreateLandlordPropertyClaimsJiraTicket([FromBody] CreateLandlordPropertyClaimsJiraTicketCommand command)
        {
            var basicInfo = await GetBasicInformation(command.LandlordId);

            if (basicInfo.Value == null)
            {
                return Conflict("Landlord has not Basic Information.");
            }
            var identityLandlord = await GetIdentityInformation(command.LandlordId);
            if (identityLandlord.Value == null)
            {
                return Conflict("Landlord has not Identity Information.");
            }

            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null && Guid.TryParse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value, out Guid userIdAux) ? userIdAux : command.LandlordId;
            string url = Url.Action("Get", "CreateLandlordPropertyClaimsJiraTicket", new { id = command.LandlordId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);

            return Accepted(context);
        }
        #endregion
    }
}
