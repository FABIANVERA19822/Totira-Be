namespace Totira.Services.RootService.Controllers
{
    using System.Web;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Totira.Services.RootService.Commands;
    using Totira.Services.RootService.DTO;
    using Totira.Services.RootService.DTO.Property;
    using Totira.Support.Api.Connection;
    using Totira.Support.Api.Controller;
    using Totira.Support.Api.Options;
    using Totira.Support.Application.Messages;
    using Totira.Support.EventServiceBus;

    [Authorize(Policy = "AppOptions")]
    public class PropertyController : DefaultBaseController
    {
        private readonly IContextFactory _contextFactory;
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly IEventBus _bus;
        private readonly ILogger<PropertyController> _logger;
        private readonly IConfiguration _configuration;

        public PropertyController(
            IEventBus bus,
            IContextFactory contextFactory,
            IQueryRestClient queryRestClient,
            IOptions<RestClientOptions> restClientOptions,
            ILogger<PropertyController> logger,
            IConfiguration configuration)
        {
            _contextFactory = contextFactory;
            _queryRestClient = queryRestClient;
            _restClientOptions = restClientOptions.Value;
            _bus = bus;
            _logger = logger;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("PropertyDetails/{ml_num}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPropertyDetailsDto>> GetProperty(string ml_num)
        {
            var PropertyData = await _queryRestClient.GetAsync<GetPropertyDetailsDto>($"{_restClientOptions.Properties}/property/propertydata/{ml_num}");

            if (PropertyData == null)
                return NotFound();

            return PropertyData;
        }

        [HttpGet("Propertylocation/{address}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPropertyLongitudeAndLatitudeDto>> Getlocation(string address)
        {
            var point = await _queryRestClient.GetAsync<GetPropertyLongitudeAndLatitudeDto>($"{_restClientOptions.ThirdPartyIntegration}/Location/propertylocation/{address}");

            if (point == null)
                return NotFound();

            return point;
        }

        [AllowAnonymous]
        [HttpGet("GetLocationsBySearchKeyWord/{SearchKeyword}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<LocationDto>>> GetLocationsBySearchKeyword(string SearchKeyword)
        {
            var Locations = await _queryRestClient.GetAsync<List<LocationDto>>($"{_restClientOptions.Properties}/property/GetLocationsBySearchKeyWord/{SearchKeyword}");

            if (Locations == null)
                return NotFound();

            return Locations;
        }

        [AllowAnonymous]
        [HttpPost("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPropertyDto>> List([FromBody] QueryProperty query)
        {
            string serializedQuery = JsonConvert.SerializeObject(query.Filter);
            string encodedQuery = HttpUtility.UrlEncode(serializedQuery);
            string url = $"{_restClientOptions.Properties}/property/list/{query.SortBy}/{query.PageNumber}/{query.PageSize}/{encodedQuery}";
            var response = await _queryRestClient.GetAsync<GetPropertyDto>(url);
            if (response == null)
                return NotFound();

            return response;
        }

        [AllowAnonymous]
        [HttpPost("map/list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GetPropertyMapDto>>> ListMap([FromBody] QueryProperty query)
        {
            string serializedQuery = JsonConvert.SerializeObject(query.Filter);
            string encodedQuery = HttpUtility.UrlEncode(serializedQuery);
            string url = $"{_restClientOptions.Properties}/property/map/{query.SortBy}/{query.PageNumber}/{query.PageSize}/{encodedQuery}";
            var response = await _queryRestClient.GetAsync<IEnumerable<GetPropertyMapDto>>(url);
            if (response is null)
                return NoContent();

            return response;
        }

        [HttpPost("CreatePropertyfromRETS")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePropertyfromMLS([FromBody] CreatePropertyfromRETSCommand command)
        {
            string url = Url.Action("Get", "Property", new { propertyType = command.PropertyType, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, Guid.Empty);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Get carrousel image by property
        /// </summary>
        /// <param name="ml_num">ml_num unique key property</param>
        /// <returns>All images from mls by property.</returns>
        [AllowAnonymous]
        [HttpGet("Property/carrousel/{ml_num}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPropertyCarrouselImagesDto>> GetPropertyCarrouselImage(string ml_num)
        {
            var CarrouselImages = await _queryRestClient.GetAsync<GetPropertyCarrouselImagesDto>($"{_restClientOptions.Properties}/property/getPropertyCarrouselImage/{ml_num}");

            if (CarrouselImages == null)
                return NotFound();

            return CarrouselImages;
        }

        [HttpPost("ImportPropertyImagesToS3")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ImportPropertyImagesToS3([FromBody] ImportPropertyImagesToS3Command command)
        {
            string url = Url.Action("Get", "ImportPropertyImagesToS3", new { propertyId = command.PropertyId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url, Guid.Empty);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }

        /// <summary>
        /// Return PropertyDetails to Apply
        /// </summary>
        /// <param name="propertyId">Id of property aka ml_num</param>
        /// <returns>Return GetPropertyDetailstoApplyDto</returns>
        [HttpGet("PropertyDetailsToApply/{propertyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPropertyDetailstoApplyDto>> GetPropertyDetailstoApply(string propertyId)
        {
            var Locations = await _queryRestClient.GetAsync<GetPropertyDetailstoApplyDto>($"{_restClientOptions.Properties}/property/propertyDetails/{propertyId}");

            if (Locations == null)
                return NotFound();

            return Locations;
        }
    }
}