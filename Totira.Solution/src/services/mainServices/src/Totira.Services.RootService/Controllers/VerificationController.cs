using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Totira.Services.RootService.Commands;
using Totira.Services.RootService.Commands.Verification;
using Totira.Services.RootService.DTO;
using Totira.Services.RootService.DTO.GroupTenant.Certn;
using Totira.Services.RootService.DTO.ThirdpartyService;
using Totira.Services.RootService.DTO.Verification;
using Totira.Services.RootService.Filters;
using Totira.Services.RootService.Options;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Controller;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using Totira.Support.ThirdPartyIntegration.Certn.Model.PropertyManagement;

namespace Totira.Services.RootService.Controllers
{
    
    public class VerificationController : DefaultBaseController
    {
        private readonly RestClientOptions _restClientOptions;
        private readonly IQueryRestClient _queryRestClient;
        private readonly IContextFactory _contextFactory;
        private readonly IEventBus _bus;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VerificationController(
            IOptions<RestClientOptions> restClientOptions,
            IQueryRestClient queryRestClient,
            IContextFactory contextFactory,
            IEventBus bus,
            IHttpContextAccessor httpContextAccessor)
        {
            _restClientOptions = restClientOptions.Value;
            _queryRestClient = queryRestClient;
            _contextFactory = contextFactory;
            _bus = bus;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet("applicants/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetCertnApplicationDto>> GetApplicant(string tenantId)
        {
            var CertnData = await _queryRestClient.GetAsync<GetCertnApplicationDto>($"{_restClientOptions.ThirdPartyIntegration}/Certn/applicants/{tenantId}");

            if (CertnData.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                var Message = CertnData.ErrorMessage.Substring(1);
                Message = Message.Substring(0, Message.Length - 1);
                return Conflict(Message);
            }
            if (CertnData == null)
                return NotFound();

            return CertnData;
        }

        [Authorize]
        [HttpPost]
        [Route("applications")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> ApplyCertnValidations([FromBody] FormBodyApplyTenantVerifications body)
        {
            if (!body.AcceptTermAndConditions)
                return Conflict("You must first accept Terms and Coditions to continue.");

            var url = $"{_restClientOptions.User}/user/tenant/{body.TenantId}/certn-request-info";
            var info = await _queryRestClient.GetAsync<GetTenantInformationForCertnApplicationDto>(url);

            if (info.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                var Message = info.ErrorMessage.Substring(1);
                Message = Message.Substring(0, Message.Length - 1);
                return Conflict(Message);
            }

            if (info.Content is null)
                return BadRequest();

            var urlContext = Url.Action("Get", "CertnRequestInfo", new { id = body.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null && Guid.TryParse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value,out Guid userIdAux) ? userIdAux : body.TenantId;
            var context = _contextFactory.Create(urlContext, userId);

            var softCheckCommand = new ApplySoftCheckCommand(body.TenantId,
                new SoftCheckRequestModel
                {
                    RequestSoftCheck = true,
                    RequestEquifax = true,
                    Information = new SoftCheckRequestModel.InformationModel()
                    {
                        FirstName = info.Content.FirstName,
                        LastName = info.Content.LastName,
                        Birthday = info.Content.Birthday,
                        SinOrSsn = info.Content.SocialInsuranceNumber,
                        Addresses = info.Content.Addresses
                            .Select(x => new InformationAddressModel
                            {
                                Address = x.Address,
                                City = x.City,
                                Country = x.Country,
                                Current = x.Current,
                                State = x.State
                            })
                            .ToList()
                    },
                    PropertyLocation = new()
                    {
                        Address = info.Content.PropertyLocation.Address,
                        City = info.Content.PropertyLocation.City,
                        County = info.Content.PropertyLocation.County,
                        Country = info.Content.PropertyLocation.Country,
                        ProvinceState = info.Content.PropertyLocation.ProvinceState,
                        LocationType = info.Content.PropertyLocation.LocationType,
                        PostalCode = info.Content.PropertyLocation.PostalCode
                    }
                });

            await _bus.PublishAsync(context, softCheckCommand);

            var validator = new UpdateTenantApplicationDetailsCommand() { IsVerificationsRequested = true, Id = body.TenantId };

            await _bus.PublishAsync(context, validator);

            return Created(urlContext, context);
        }

        
        [HttpPost]
        [Route("applications/jira")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendToJira([FromBody] TenantEmployeeAndIncomeTicketJiraCommand body)
        {
            var urlContext = Url.Action("Get", "PersonaRequestInfo", new { id = body.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : body.TenantId;
            var context = _contextFactory.Create(urlContext, userId);

            await _bus.PublishAsync(context, body);

            var currentJobProfile = new UpdateTenantCurrentJobStatusCommand {
                UpdateCurrentJobStatus = false,
                CurrentJobStatus = "",
                IsUnderRevisionSend = true,
                TenantId = body.TenantId,
            };
            await _bus.PublishAsync(context, currentJobProfile);
            return Ok();
        }

        [HttpPost("ProfileInterestJiraTicket")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> SendProfileInterestJiraTicket([FromBody] CreateProfileInterestJiraTicketCommand command)
        {
            string url = Url.Action("Get", "ProfileInterest", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null && Guid.TryParse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value,out Guid userIdAux) ? userIdAux : command.TenantId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [HttpPost("GroupProfileInterestJiraticket")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> SendGroupProfileInterestJiraticket([FromBody] CreateGroupProfileInterestJiraticketCommand command)
        {
            string url = Url.Action("Get", "GroupProfileInterest", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null && Guid.TryParse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value,out Guid userIdAux) ? userIdAux : Guid.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [HttpPost]
        [Route("applications/update/jira")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ServiceFilter(typeof(SafeIPsListFilter), IsReusable = true, Order = 1)]
        [ServiceFilter(typeof(ApiKeyAuthorizationFilter), IsReusable = true, Order = 2)]        
        public async Task<IActionResult> UpdateOnJiraTicket([FromBody] object obj)
        {
            var json = obj.ToString();
            var body = JsonConvert.DeserializeObject<UpdateTenantEmployeeAndIncomeTicketJiraCommand>(json);

            var urlContext = Url.Action("Get", "PersonaRequestInfo", new { id = body.Issue.id, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : Guid.Empty;
            var context = _contextFactory.Create(urlContext, userId);

            await _bus.PublishAsync(context, body);
            return Ok();
        }

        [HttpPost]
        [Route("applications/persona")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ServiceFilter(typeof(SafeIPsListFilter), IsReusable = true, Order = 1)]
        [ServiceFilter(typeof(ApiKeyAuthorizationFilter), IsReusable = true, Order = 2)]
        public async Task<IActionResult> ApplyPersonaValidations([FromBody] TenantPersonaValidationCommand body)
        {
            var urlContext = Url.Action("Get", "PersonaRequestInfo", new { id = body.data.id, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : Guid.Parse(body.data.attributes.payload.data.attributes.referenceid);
            var context = _contextFactory.Create(urlContext, userId);

            await _bus.PublishAsync(context, body);

            return Ok();
        }

        [HttpGet("applicants/persona/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetPersonaApplicationDto>> GetPersonaResult(Guid tenantId)
        {
            var personaData = await _queryRestClient.GetAsync<GetPersonaApplicationDto>($"{_restClientOptions.ThirdPartyIntegration}/persona/applicants/{tenantId}");

            if (personaData == null)
                return NotFound();

            return personaData;
        }

        [HttpGet("applicants/jira/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetJiraEmployementTicketDto>> GetJiraResult(Guid tenantId)
        {
            var jiraData = await _queryRestClient.GetAsync<GetJiraEmployementTicketDto>($"{_restClientOptions.ThirdPartyIntegration}/jira/applicants/{tenantId}");

            if (jiraData == null)
                return NotFound();

            return jiraData;
        }

        [HttpGet("applicants/VerifiedProfile/profile/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetTenantVerifiedProfileDto>> GetVerificationsForTenant(Guid tenantId)
        {
            var personaData = await _queryRestClient.GetAsync<GetTenantVerifiedProfileDto>($"{_restClientOptions.ThirdPartyIntegration}/VerifiedProfile/profiles/{tenantId}");

            if (personaData == null)
                return NotFound();

            return personaData;
        }

        [HttpPost("applications-groups/certn")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ApplyCertnGroupValidations([FromBody]FormBodyApplyGroupTenantVerifications request)
        {
            if (!request.AcceptTermAndConditions)
                return Conflict("You must first accept Terms and Coditions to continue.");

            var url = $"{_restClientOptions.User}/group/main-tenant/{request.MainTenantId}/certn/request-info";
            var response = await _queryRestClient.GetAsync<GetGroupApplicantsInfoByTenantIdDto>(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content.Completed == "None")
                    return Conflict("At least one applicant must have info before send it.");

                var certnCommand = new ApplyCertnGroupCommand();

                foreach (var coapplicant in response.Content.Applicants.Where(x => x.Status == "Complete" && x.Information is not null))
                {
                    certnCommand.TenantGroups.Add(new()
                    {
                        TenantId = coapplicant.Information!.TenantId,
                        RequestModel = new()
                        {
                            RequestSoftCheck = true,
                            RequestEquifax = true,
                            Information = new SoftCheckRequestModel.InformationModel()
                            {
                                FirstName = coapplicant.Information.FirstName,
                                LastName = coapplicant.Information.LastName,
                                Birthday = coapplicant.Information.Birthday,
                                SinOrSsn = coapplicant.Information.SocialInsuranceNumber,
                                Addresses = coapplicant.Information.Addresses
                                    .Select(x => new InformationAddressModel
                                    {
                                        Address = x.Address,
                                        City = x.City,
                                        Country = x.Country,
                                        Current = x.Current,
                                        State = x.State
                                    })
                                    .ToList()
                            },
                            PropertyLocation = new()
                            {
                                Address = coapplicant.Information.PropertyLocation.Address,
                                City = coapplicant.Information.PropertyLocation.City,
                                County = coapplicant.Information.PropertyLocation.County,
                                Country = coapplicant.Information.PropertyLocation.Country,
                                ProvinceState = coapplicant.Information.PropertyLocation.ProvinceState,
                                LocationType = coapplicant.Information.PropertyLocation.LocationType,
                                PostalCode = coapplicant.Information.PropertyLocation.PostalCode
                            }
                        }
                    });
                }

                var urlContext = Url.Action("Get", "PersonaRequestInfo", new { id = request.MainTenantId, version = API_VERSION }) ?? string.Empty;
                var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : Guid.Empty;
                var context = _contextFactory.Create(urlContext, userId);

                await _bus.PublishAsync(context, certnCommand);

                var verificationCommand = new VerifyTenantGroupCommand()
                {
                    MainTenantId = request.MainTenantId,
                    RequestVerification = true
                };

                await _bus.PublishAsync(context, verificationCommand);

                return Accepted(context);
            }

            return Conflict($"User service is not returning OK status. {response.ErrorMessage}");
        }

        [HttpPost("application-groups/jira")]
        [ProducesResponseType(typeof(IContext), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SendGroupToJira([FromBody]FormBodyApplyJiraGroupTenantVerification request)
        {
            var url = $"{_restClientOptions.User}/group/main-tenant/{request.MainTenantId}/certn/request-info";
            var response = await _queryRestClient.GetAsync<GetGroupApplicantsInfoByTenantIdDto>(url);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content.Completed == "None")
                    return Conflict("At least one applicant must have info before send it.");

                var urlContext = Url.Action("Get", "PersonaRequestInfo", new { id = request.MainTenantId, version = API_VERSION }) ?? string.Empty;
                var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : request.MainTenantId;
                var context = _contextFactory.Create(urlContext, userId);

                var command = new ApplyJiraGroupCommand();
                command.Tenants.AddRange(response.Content.Applicants
                    .Where(x => x.Status == "Complete" && x.Information is not null)
                    .Select(x => x.Information!.TenantId));

                await _bus.PublishAsync(context, command);

                if (request.IsReVerificationRequested)
                {
                    var verificationCommand = new VerifyTenantGroupCommand()
                    {
                        MainTenantId = request.MainTenantId,
                        RequestReVerification = true
                    };

                    await _bus.PublishAsync(context, verificationCommand);
                }

                return Accepted(context);
            }
            else
            {
                return Conflict($"User service is not returning OK status.");
            }
        }
    }
}
