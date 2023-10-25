

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Totira.Services.RootService.Commands;
using Totira.Services.RootService.DTO;
using Totira.Services.RootService.DTO.Common;
using Totira.Services.RootService.Queries;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Controller;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;

using Totira.Support.CommonLibrary.Interfaces;

namespace Totira.Services.RootService.Controllers
{
    public class TenantController : DefaultBaseController
    {
        private readonly IContextFactory _contextFactory;
        private readonly IQueryRestClient _queryRestClient;
        private readonly IEventBus _bus;
        private readonly RestClientOptions _restClientOptions;
        private readonly ILogger<TenantController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMaskObject _maskObject;

        public TenantController(
            IQueryRestClient queryRestClient,
            IEventBus bus,
            IOptions<RestClientOptions> restClientOptions,
            IContextFactory contextFactory,
            ILogger<TenantController> logger,
            IHttpContextAccessor httpContextAccessor,
            IMaskObject maskObject)
        {
            _contextFactory = contextFactory;
            _queryRestClient = queryRestClient;
            _bus = bus;
            _restClientOptions = restClientOptions.Value;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _maskObject = maskObject;
        }

        #region Basic Information

        [Authorize(Policy = "AppOptions")]
        [HttpGet("{id}/BasicInfo")]
        [ProducesResponseType(typeof(GetTenantBasicInformationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, MediaTypeNames.Text.Plain)]
        public async Task<ActionResult<GetTenantBasicInformationDto>> GetBasicInformation(Guid id)
        {
            var url = $"{_restClientOptions.User}/user/tenant/{id}/basic-info";
            var basicInfo = await _queryRestClient.GetAsync<GetTenantBasicInformationDto>(url);

            if (basicInfo is null)
                return NotFound();

            return basicInfo;
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPost("BasicInfo")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IContext), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Post([FromBody] CreateTenantBasicInformationCommand command)
        {
            string url = Url.Action("Get", "UserPersonal", new { id = command.Id, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.Id;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPut("BasicInfo")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IContext), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Put([FromBody] UpdateTenantBasicInformationCommand command)
        {
            string url = Url.Action("Get", "UserPersonal", new { id = command.Id, version = API_VERSION }) ?? string.Empty;

            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.Id;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        #endregion Basic Information

        [Authorize(Policy = "AppOptions")]
        [HttpGet("AcquaintanceReferral/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantaAquaintanceReferralDto>> GetTenantaAquaintanceReferral(Guid id)
        {
            var AcquaintanceReferral = await _queryRestClient.GetAsync<GetTenantaAquaintanceReferralDto>($"{_restClientOptions.User}/user/tenant/AcquaintanceReferral/{id}");
            if (AcquaintanceReferral == null)
            {
                return NotFound();
            }
            else
            {
                return AcquaintanceReferral;
            }
        }


        [HttpGet("ProfileSummary/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<GetTenantProfileSummaryDto>> GetTenantProfileSummary(Guid id)
        {
            var isAuthenticatedUser = _httpContextAccessor.HttpContext.Items["tenantId"] == null ? false : true;
            var ProfileSummary = await _queryRestClient.GetAsync<GetTenantProfileSummaryDto>($"{_restClientOptions.User}/user/tenant/ProfileSummary/{id}");
            if (ProfileSummary == null)
            {
                return NotFound();
            }
            else
            {
                if (!isAuthenticatedUser)
                {
                    MaskProperties(ProfileSummary);
                    
                }
                return ProfileSummary;
            }
        }

        private void MaskProperties(QueryResponse<GetTenantProfileSummaryDto> ProfileSummary)
        {
            ProfileSummary.Content.ContactInformation = _maskObject.ModifyPropertiesToAsterisk<GetTenantContactInformationDto>(ProfileSummary.Content.ContactInformation, typeof(MaskAttribute));
            ProfileSummary.Content.EmployeeIncomes.CurrentEmployements = _maskObject.ModifyPropertiesListObjectToAsterisk<CurrentEmploymentDto>(ProfileSummary.Content.EmployeeIncomes.CurrentEmployements, typeof(MaskAttribute));
            ProfileSummary.Content.EmployeeIncomes.PastEmployments = _maskObject.ModifyPropertiesListObjectToAsterisk<PastEmploymentDto>(ProfileSummary.Content.EmployeeIncomes.PastEmployments, typeof(MaskAttribute));
            ProfileSummary.Content.RentalHistories.RentalHistories = _maskObject.ModifyPropertiesListObjectToAsterisk<TenantRentalHistoryDto>(ProfileSummary.Content.RentalHistories.RentalHistories, typeof(MaskAttribute));
            ProfileSummary.Content.OtherReferrals = _maskObject.ModifyPropertiesListObjectToAsterisk<GetAcquaintanceReferralFormInfoDto>(ProfileSummary.Content.OtherReferrals, typeof(MaskAttribute));
            ProfileSummary.Content.BasicInformation = _maskObject.ModifyPropertiesToAsterisk<GetTenantBasicInformationDto>(ProfileSummary.Content.BasicInformation, typeof(MaskAttribute));

        }

        [Authorize(Policy = "AppOptions")]
        [HttpGet("GroupApplicationSummary/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantGroupApplicationSummaryDto>> GetTenantGroupApplicationSummary(Guid id)
        {
            var GroupApplicationSummary = await _queryRestClient.GetAsync<GetTenantGroupApplicationSummaryDto>($"{_restClientOptions.User}/user/tenant/GroupApplicationSummary/{id}");
            if (GroupApplicationSummary == null)
            {
                return NotFound();
            }
            else
            {
                return GroupApplicationSummary;
            }
        }

        [Authorize(Policy = "AppOptions")]
        [HttpGet("ApplicationListPage/{id}/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantApplicationListPageDto>> GetTenantApplicationListPage(Guid id, int pageNumber, int pageSize)
        {
            var ApplicationListPage = await _queryRestClient.GetAsync<GetTenantApplicationListPageDto>($"{_restClientOptions.User}/user/tenant/ApplicationListPage/{id}/{pageNumber}/{pageSize}");
            if (ApplicationListPage == null)
            {
                return NotFound();
            }
            else
            {
                return ApplicationListPage;
            }
        }
        [Authorize(Policy = "AppOptions")]
        [HttpPost("ContactLandlord")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveTenantContactLandlord([FromBody] CreateTenantContactLandlordCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "ContactLandlord", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId == null? Guid.Empty : userId.Value);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpDelete("{mainTenantId}/{applicationRequestId}/coSigner/{coSignerId}/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveGuarantorToApplicationRequest(Guid applicationRequestId, Guid mainTenantId, Guid coSignerId)
        {
            DeleteTenantCoSignerFromGroupApplicationProfileCommand command = new DeleteTenantCoSignerFromGroupApplicationProfileCommand { MainTenantId = mainTenantId, ApplicationRequestId = applicationRequestId, CoSignerId = coSignerId };
            string url = Url.Action("Get", "GroupApplication", new { id = command.MainTenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.MainTenantId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize]
        [HttpGet("AcquaintanceReferralEmails/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantaAquaintanceReferralEmailsDto>> GetTenantaAquaintanceReferralEmails(Guid id)
        {
            var AcquaintanceReferralEmails = await _queryRestClient.GetAsync<GetTenantaAquaintanceReferralEmailsDto>($"{_restClientOptions.User}/user/tenant/AcquaintanceReferralEmails/{id}");
            if (AcquaintanceReferralEmails == null)
            {
                return NotFound();
            }
            else
            {
                return AcquaintanceReferralEmails;
            }
        }

        [Authorize(Policy = "AppOptions")]
        [HttpGet("{id}/ApplicationDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantApplicationDetailsDto>> GetTenantApplicationDetails(Guid id)
        {
            var basicInfo = await _queryRestClient.GetAsync<GetTenantApplicationDetailsDto>($"{_restClientOptions.User}/user/tenant/{id}/ApplicationDetails");
            if (basicInfo == null)
            {
                return NotFound();
            }
            else
            {
                return basicInfo;
            }
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPost("ApplicationDetails")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveApplicationDetails([FromBody] CreateTenantApplicationDetailsCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.Id;
            string url = Url.Action("Get", "UserPersonal", new { id = command.Id, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPost("AcquaintanceReferral")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveAcquaintanceReferral([FromBody] CreateTenantAcquaintanceReferralCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "referals", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPut("ApplicationDetails")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateApplicationDetails([FromBody] UpdateTenantApplicationDetailsCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.Id;
            string url = Url.Action("Get", "UserPersonal", new { id = command.Id, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPut("AcquaintanceReferral")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAcquaintanceReferral([FromBody] UpdateTenantAcquaintanceReferralCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "referals", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPut("AcquaintanceReferral/reactivate")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReactivateAquitanceReferral([FromBody] UpdateTenantAcquaintanceReferralReactivateCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : Guid.Empty;
            string url = Url.Action("Get", "referals", new { id = command.ReferralId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPost("RentalHistories")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveRentalHistories([FromBody] CreateTenantRentalHistoriesCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "RentalHistories", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPut("RentalHistories")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRentalHistories([FromBody] UpdateTenantRentalHistoriesCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "RentalHistories", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPut("RentalHistories/reactivate")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReactivateRentalHistories([FromBody] UpdateTenantRentalHistoriesReactivateCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : Guid.Empty;
            string url = Url.Action("Get", "RentalHistories", new { id = command.LandlordId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize]
        [HttpPost("TenantFeedbackViaLandlord")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TenantFeedbackViaLandlord([FromBody] CreateTenantFeedbackViaLandlordCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null && Guid.TryParse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value,out Guid userIdAux) ? userIdAux : command.TenantId;
            string url = Url.Action("Get", "TenantFeedback", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize]
        [HttpGet("TenantFeedbackViaLandlord/{landlordId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantFeedbackViaLandlordDto>> GetTenantFeedbackViaLandlord(Guid landlordId)
        {
            var FeedbackInfo = await _queryRestClient.GetAsync<GetTenantFeedbackViaLandlordDto>($"{_restClientOptions.User}/user/tenant/TenantFeedbackViaLandlord/{landlordId}");

            if (FeedbackInfo == null)
            {
                return NotFound();
            }
            else
            {
                return FeedbackInfo;
            }
        }

        [Authorize(Policy = "AppOptions")]
        [HttpGet("RentalHistories/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantRentalHistoriesDto>> GetTenantRentalHistories(Guid id)
        {
            var RentalHistories = await _queryRestClient.GetAsync<GetTenantRentalHistoriesDto>($"{_restClientOptions.User}/user/tenant/RentalHistories/{id}");
            if (RentalHistories == null)
            {
                return NotFound();
            }
            else
            {
                return RentalHistories;
            }
        }

        #region Tenant incomes

        /// <summary>
        /// Get tenant incomes.
        /// </summary>
        /// <param name="tenantId">Tenant id</param>
        /// <returns>All tenant income types list.</returns>
        [Authorize(Policy = "AppOptions")]
        [HttpGet("{tenantId}/EmployeeIncomes")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(GetTenantEmployeeIncomesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantEmployeeIncomesDto>> GetTenantEmployeeIncomes(Guid tenantId)
        {
            var url = $"{_restClientOptions.User}/user/tenant/{tenantId}/incomes";
            var incomes = await _queryRestClient.GetAsync<GetTenantEmployeeIncomesDto>(url);

            if (incomes == null)
                return NotFound();

            return incomes;
        }

        [Authorize(Policy = "AppOptions")]
        [HttpGet("ShareProfile/{tenantId}/{encryptedAccessCode}/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantShareProfileDto>> GetTenantShareProfile(Guid tenantId, string encryptedAccessCode, string email)
        {
            var shareProfile = await _queryRestClient.GetAsync<GetTenantShareProfileDto>($"{_restClientOptions.User}/user/tenant/{tenantId}/{encryptedAccessCode}/{email}/shareProfile");
            if (shareProfile == null)
            {
                return NotFound();
            }
            else
            {
                return shareProfile;
            }
        }

        /// <summary>
        /// Gets tenant employee income information for form.
        /// </summary>
        /// <param name="tenantId">Tenant id</param>
        /// <param name="incomeId">Income id</param>
        /// <returns>Tenant employee income data.</returns>
        [Authorize(Policy = "AppOptions")]
        [HttpGet("{tenantId}/EmployeeIncomes/{incomeId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(GetTenantEmployeeIncomeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetTenantEmployeeIncomeDto>> GetTenantEmployeeIncomeById(Guid tenantId, Guid incomeId)
        {
            var url = $"{_restClientOptions.User}/user/tenant/{tenantId}/employee-incomes/{incomeId}";
            var employeeIncome = await _queryRestClient.GetAsync<GetTenantEmployeeIncomeDto>(url);
            if (employeeIncome is null)
                return NotFound();

            return employeeIncome;
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPost("EmployeeIncomes")]
        [Consumes(typeof(FormCreateTenantEmployeeIncomesDto), "multipart/form-data")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveEmployeeIncomes([FromForm] FormCreateTenantEmployeeIncomesDto dto)
        {
            string url = Url.Action("Get", "EmployeeIncomes", new { id = dto.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : dto.TenantId;
            var context = _contextFactory.Create(url, userId);

            var createCommand = new CreateTenantEmployeeIncomesCommand()
            {
                TenantId = dto.TenantId,
                CompanyOrganizationName = dto.CompanyOrganizationName,
                Position = dto.Position,
                MonthlyIncome = dto.MonthlyIncome,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ContactReference = dto.ContactReference,
                IsCurrentlyWorking = dto.IsCurrentlyWorking,
            };

            foreach (var formFile in dto.Files)
            {
                using var ms = new MemoryStream();
                await formFile.CopyToAsync(ms);
                createCommand.Files.Add(new EmployeeIncomeFile(
                    formFile.FileName,
                    formFile.Length,
                    formFile.ContentType,
                    ms.ToArray()));
            }

            await _bus.PublishAsync(context, createCommand);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPut("EmployeeIncomes")]
        [Consumes(typeof(FormUpdateTenantEmployeeIncomesDto), "multipart/form-data")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEmployeeIncomes([FromForm] FormUpdateTenantEmployeeIncomesDto dto)
        {
            string url = Url.Action("Get", "EmployeeIncomes", new { id = dto.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : dto.TenantId;
            var context = _contextFactory.Create(url, userId);

            var updateCommand = new UpdateTenantEmployeeIncomesCommand()
            {
                IncomeId = dto.IncomeId,
                TenantId = dto.TenantId,
                CompanyOrganizationName = dto.CompanyOrganizationName,
                Position = dto.Position,
                MonthlyIncome = dto.MonthlyIncome,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsCurrentlyWorking = dto.IsCurrentlyWorking,
                ContactReference = dto.ContactReference,
                DeletedFiles = dto.DeletedFiles,
            };

            // Llenamos la propiedad Files
            foreach (var formFile in dto.Files)
            {
                using var ms = new MemoryStream();
                await formFile.CopyToAsync(ms);
                updateCommand.NewFiles.Add(new EmployeeIncomeFile(
                    formFile.FileName,
                    formFile.Length,
                    formFile.ContentType,
                    ms.ToArray()));
            }

            await _bus.PublishAsync(context, updateCommand);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpDelete]
        [Route("EmployeeIncomes/Files/{TenantId}/{IncomeId}/{FileName}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteEmployeeIncomesFiles(Guid TenantId, Guid IncomeId, string FileName)
        {
            DeleteTenantEmployeeIncomeFileCommand command = new DeleteTenantEmployeeIncomeFileCommand(TenantId, IncomeId, FileName);
            string url = Url.Action("Get", "EmployeeIncomes", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : TenantId;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpDelete]
        [Route("EmployeeIncomes/Income/{TenantId}/{IncomeId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteEmployeeIncome(Guid TenantId, Guid IncomeId)
        {
            DeleteTenantEmployeeIncomeIdCommand command = new DeleteTenantEmployeeIncomeIdCommand(TenantId, IncomeId);
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : TenantId;
            string url = Url.Action("Get", "EmployeeIncomes", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        #endregion Tenant incomes

        #region Student Tenants

        [Authorize(Policy = "AppOptions")]
        [HttpGet]
        [Route("{tenantId}/study-details/{studyId}")]
        [ProducesResponseType(typeof(GetTenantStudentDetailByIdDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<GetTenantStudentDetailByIdDto>> GetStudentDetailById(Guid tenantId, Guid studyId)
        {
            var url = $"{_restClientOptions.User}/user/tenant/{tenantId}/study-details/{studyId}";
            var studyDetail = await _queryRestClient.GetAsync<GetTenantStudentDetailByIdDto>(url);
            if (studyDetail is null)
                return NoContent();

            return studyDetail;
        }

        [Authorize(Policy = "AppOptions")]
        [HttpDelete]
        [Route("{tenantId}/study-details/{studyId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteTenantStudentDetailById(Guid tenantId, Guid studyId)
        {
            if (_httpContextAccessor.HttpContext is null)
                return BadRequest();

            var claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim is not null
                ? Guid.Parse(claim.Value)
                : tenantId;
            string url = Url.Action("Get", "study-details", new { id = tenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            var command = new DeleteTenantStudentDetailCommand(tenantId, studyId);

            await _bus.PublishAsync(context, command);

            return Accepted();
        }

        [Authorize(Policy = "AppOptions")]
        [HttpDelete]
        [Route("{tenantId}/study-details/{studyId}/files/{fileName}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteStudentFinancialDetailFile(Guid tenantId, Guid studyId, string fileName)
        {
            var url = Url.Action("Get", "study-details", new { id = tenantId, version = API_VERSION }) ?? string.Empty;

            if (_httpContextAccessor.HttpContext is null)
                return BadRequest();

            var claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim is not null
                ? Guid.Parse(claim.Value)
                : tenantId;

            var context = _contextFactory.Create(url, userId);
            var command = new DeleteTenantStudentFinancialDetailFileCommand(tenantId, studyId, fileName);

            await _bus.PublishAsync(context, command);

            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPost]
        [Route("study-details")]
        [Consumes(typeof(FormCreateTenantStudentFinancialDetailDto), "multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveTenantStudentDetails([FromForm] FormCreateTenantStudentFinancialDetailDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (_httpContextAccessor.HttpContext is null)
                return BadRequest();

            var claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim is not null
                ? Guid.Parse(claim.Value)
                : dto.TenantId;

            var url = Url.Action("Get", "study-details", new { id = dto.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            Func<IFormFile, FileInfoDto> files = (file => new FileInfoDto(file));

            var createCommand = new CreateTenantStudentFinancialDetailCommand(
                dto.TenantId,
                dto.UniversityOrInstitute,
                dto.Degree,
                dto.IsOverseasStudent,
                dto.EnrollmentProofs.Select(files).ToList(),
                dto.StudyPermitsOrVisas?.Select(files).ToList(),
                dto.IncomeProofs.Select(files).ToList());

            await _bus.PublishAsync(context, createCommand);

            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPut]
        [Route("study-details")]
        [Consumes(typeof(FormUpdateTenantStudentFinancialDetailDto), "multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTenantStudentDetails([FromForm] FormUpdateTenantStudentFinancialDetailDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (_httpContextAccessor.HttpContext is null)
                return BadRequest();

            var claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim is not null
                ? Guid.Parse(claim.Value)
                : dto.TenantId;

            var url = Url.Action("Get", "study-details", new { id = dto.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            Func<IFormFile, FileInfoDto> items = (file => new FileInfoDto(file));

            var command = new UpdateTenantStudentFinancialDetailCommand(
                dto.TenantId,
                dto.StudyId,
                dto.UniversityOrInstitute,
                dto.Degree,
                dto.IsOverseasStudent,
                dto.DeletedEnrollmentProofs.ToList(),
                dto.EnrollmentProofs.Select(items).ToList(),
                dto.DeleteStudyPermitsOrVisas.ToList(),
                dto.StudyPermitsOrVisas.Select(items).ToList(),
                dto.DeleteIncomeProofs.ToList(),
                dto.IncomeProofs.Select(items).ToList());

            await _bus.PublishAsync(context, command);

            return Accepted(context);
        }

        #endregion Student Tenants

        [Authorize(Policy = "AppOptions")]
        [HttpPost("EmploymentReference")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveEmploymentReference([FromBody] CreateTenantEmploymentReferenceCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "EmploymentReference", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPut("EmploymentReference")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEmploymentReference([FromBody] UpdateTenantEmploymentReferenceCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "EmploymentReference", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpGet("EmploymentReference/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantEmploymentReferenceDto>> GetTenantEmploymentReference(Guid id)
        {
            var RentalHistories = await _queryRestClient.GetAsync<GetTenantEmploymentReferenceDto>($"{_restClientOptions.User}/user/tenant/EmploymentReference/{id}");
            if (RentalHistories == null)
            {
                return NotFound();
            }
            else
            {
                return RentalHistories;
            }
        }

        [Authorize(Policy = "AppOptions")]
        [HttpGet("ContactInfo/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTenantContactInformationDto>> GetTenantContactInformation(Guid tenantId)
        {
            var contactInfo = await _queryRestClient.GetAsync<GetTenantContactInformationDto>($"{_restClientOptions.User}/user/tenant/contactinfo/{tenantId}");
            if (contactInfo == null)
            {
                return NotFound();
            }
            else
            {
                return contactInfo;
            }
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPost("ContactInfo")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveTenantContactInfo([FromBody] CreateTenantContactInformationCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "ContactInfo", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPut("ContactInfo")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTenantContactInfo([FromBody] UpdateTenantContactInformationCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "ContactInfo", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Method to share profile of single tenant verified via email
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Policy = "AppOptions")]
        [HttpPost("ShareProfile")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveTenantShareProfile([FromBody] CreateTenantShareProfileCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "ShareProfile", new { id = command.TenantId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [HttpGet("{tenantId}/terms-and-conditions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<GetTermsAndConditionsByTenantIdDto>> GetTermsAndConditions(Guid tenantId)
        {
            var termsAndConditions = await _queryRestClient.GetAsync<GetTermsAndConditionsByTenantIdDto>($"{_restClientOptions.User}/user/tenant/{tenantId}/terms-and-conditions");
            if (termsAndConditions is null)
                return NotFound();
            else
                return termsAndConditions;
        }

        [Authorize(Policy = "AppOptions")]
        [HttpPost("AcceptTermsAndConditions")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<IActionResult> AcceptTermsAndConditions([FromBody] AcceptTermsAndConditionsCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.TenantId;
            string url = Url.Action("Get", "AcceptTermsAndConditions", new { tenantId = command.TenantId, signingDateTime = command.SigningDateTime, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }
                
        [HttpGet("ValidateLinkOtp/{otpId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ValidateLinkOtpDto>> ValidateLinkOtp(Guid otpId)
        {
            var response = await _queryRestClient.GetAsync<ValidateLinkOtpDto>($"{_restClientOptions.User}/User/ValidateLinkOtp/{otpId}");
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return response;
            }
        }
        [Authorize(Policy = "PublicOptions")]
        [HttpPost("ValidateOtp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ValidateOtpDto>> ValidateOtp([FromBody] QueryValidateOtp request)
        {
            var response = await _queryRestClient.GetAsync<ValidateOtpDto>($"{_restClientOptions.User}/User/ValidateOtp/{request.OtpId}/{request.AccessCode}");
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return response;
            }
        }

        [HttpPost("UpdateTenantShareProfileTerms")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateTenantShareProfileTerms([FromBody] UpdateTenantShareProfileTermsCommand command)
        {

            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null && Guid.TryParse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value, out Guid userIdAux) ? userIdAux : command.Id;
            string url = Url.Action("Get", "UpdateTenantShareProfileTerms", new { id = command.Id, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        [Authorize(Policy = "AppOptions")]
        [HttpGet("GetVerificationProcessStatus/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetVerificationProcessStatus(Guid tenantId)
        {
            var status = await _queryRestClient.GetAsync<string>($"{_restClientOptions.User}/user/GetVerificationProcessStatus/{tenantId}");
            if (status == null)
            {
                return NotFound();
            }
            else
            {
                return status;
            }
        }

        
        [HttpGet("TenantVerifiedProfile/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<GetTenantVerifiedbyProfileDto>> GetTenantVerifiedProfile(Guid tenantId)
        {
            var verifiedProfile = await _queryRestClient.GetAsync<GetTenantVerifiedbyProfileDto>($"{_restClientOptions.User}/user/tenant/verifiedprofile/{tenantId}");
            if (verifiedProfile == null)
            {
                return NotFound();
            }
            else
            {
                return verifiedProfile;
            }
        }

        [Authorize]
        [HttpGet("CheckUserIsExistByEmail/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUserIsExistByEmailDto>> GetTenantContactInformation(string email)
        {
            var query = await _queryRestClient.GetAsync<GetUserIsExistByEmailDto>($"{_restClientOptions.User}/user/CheckUserIsExistByEmail/{email}");
            if (query == null)
            {
                return NotFound();
            }
            else
            {
                return query;
            }
        }

        [Authorize(Policy = "AppOptions")]
        [HttpDelete("{mainTenantId}/{applicationRequestId}/coSigner/{coSignerId}/removeForGroup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LeaveCosignersFromApplicationRequest(Guid applicationRequestId, Guid mainTenantId, Guid coSignerId)
        {
            DeleteCosignersFromGroupApplicationCommand command = new DeleteCosignersFromGroupApplicationCommand { MainTenantId = mainTenantId, ApplicationRequestId = applicationRequestId, CoSignerId = coSignerId };
            string url = Url.Action("Get", "GroupApplication", new { id = command.MainTenantId, version = API_VERSION }) ?? string.Empty;
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.MainTenantId;
            var context = _contextFactory.Create(url, userId);

            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Return CurrentJobStatus by tenantId
        /// </summary>
        /// <param name="tenantId">Owner of application</param>
        /// <returns>Return CurrentJobStatus</returns>
        [HttpGet("CurrentJobStatus/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<GetTenantCurrentJobStatusByDto>> GetTenantCurrentJobStatusByTenantId(Guid tenantId)
        {
            var status = await _queryRestClient.GetAsync<GetTenantCurrentJobStatusByDto>($"{_restClientOptions.User}/user/GetTenantCurrentJobStatusByTenantId/{tenantId}");
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
        ///  Save tenant application of property listing
        /// </summary> 
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("PropertyToApply")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AppOptions")]
        public async Task<ActionResult> SavePropertytoApply([FromBody] CreatePropertyToApplyCommand command)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier) != null ? Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value) : command.ApplicantId;
            string url = Url.Action("Get", "SavePropertytoApply", new { propertyId = command.PropertyId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, userId);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Return PropertyApplication by PropertyId, ApplicationRequest
        /// </summary>
        /// <param name="propertyId">Owner of application</param>
        /// <param name="applicationRequestId">Owner of application</param>
        /// <returns></returns>
        [HttpGet("PropertyApplied/{propertyId}/{applicationRequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<GetPropertyAppliedDto>> GetPropertyApplied(string propertyId, Guid applicationRequestId)
        {
            var status = await _queryRestClient.GetAsync<GetPropertyAppliedDto>($"{_restClientOptions.User}/user/GetPropertyAppliedby/{propertyId}/{applicationRequestId}");
            if (status == null)
            {
                return NotFound();
            }
            else
            {
                return status;
            }
        }

    }
}