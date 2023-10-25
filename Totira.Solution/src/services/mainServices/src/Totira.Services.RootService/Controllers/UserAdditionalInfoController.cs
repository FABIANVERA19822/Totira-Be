using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.ViewModels;
using Totira.Support.Api.Options;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Controller;
using Totira.Support.EventServiceBus;

namespace Totira.Services.RootService.Controllers
{


    public class UserAdditionalInfoController : DefaultBaseController
    {
        private readonly IQueryRestClient _queryRestClient;
        private readonly IEventBus _bus;
        private readonly RestClientOptions _restClientOptions;

        public UserAdditionalInfoController(
            IQueryRestClient queryRestClient,
            IEventBus bus,
            IOptions<RestClientOptions> restClientOptions)
        {
            _queryRestClient = queryRestClient;
            _bus = bus;
            _restClientOptions = restClientOptions.Value;

        }
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUserAdditionalInfoDto>> Get()
        {
            var user = await _queryRestClient.GetAsync<GetUserAdditionalInfoDto>($"{_restClientOptions.User}/useradditionalinfo");
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return user;
            }
        }
    }
}
