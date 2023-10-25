using Microsoft.AspNetCore.Mvc;

namespace Totira.Support.Api.Controller
{
    [ApiController]
    [ApiVersion("1")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DefaultBaseController : ControllerBase
    {
        public const string API_VERSION = "1";
    }
}
