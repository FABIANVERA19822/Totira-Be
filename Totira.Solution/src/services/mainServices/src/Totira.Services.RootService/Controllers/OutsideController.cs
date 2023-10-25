using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Totira.Services.RootService.Commands;
using Totira.Services.RootService.DTO;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Controller;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.EventServiceBus;

namespace Totira.Services.RootService.Controllers
{
    public class OutsideController : DefaultBaseController
    {
        private readonly IContextFactory _contextFactory;
        private readonly IQueryRestClient _queryRestClient;
        private readonly IEventBus _bus;
        private readonly RestClientOptions _restClientOptions;
        private readonly ILogger<OutsideController> _logger;

        private readonly ISecurityHandler _sec;

        private const string API_VERSION = "1";

        private static readonly HttpClient client = new HttpClient();

        public OutsideController(
           IQueryRestClient queryRestClient,
           IEventBus bus,
           IOptions<RestClientOptions> restClientOptions,
           IContextFactory contextFactory,
           ILogger<OutsideController> logger,
           ISecurityHandler sec)
        {
            _contextFactory = contextFactory;
            _queryRestClient = queryRestClient;
            _bus = bus;
            _restClientOptions = restClientOptions.Value;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<GetAcquaintanceReferralFormInfoDto>> GetTenantBasicInformation(Guid id)
        {
            var basicInfo = await _queryRestClient.GetAsync<GetAcquaintanceReferralFormInfoDto>($"{_restClientOptions.User}/ReferralInfo/{id}");
            if (basicInfo == null)
            {
                return NotFound();
            }
            else
            {
                return basicInfo;
            }
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreateAcquaintanceReferralFormInfoCommand command)
        {
            string url = Url.Action("Get", "AcquaintanceReferralFormInfo", new { id = command.ReferralId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, Guid.Empty);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }
                
    }
}