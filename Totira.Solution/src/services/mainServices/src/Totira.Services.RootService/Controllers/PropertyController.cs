namespace Totira.Services.RootService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System.Web;
    using Totira.Services.RootService.Commands;
    using Totira.Services.RootService.DTO;
    using Totira.Support.Api.Connection;
    using Totira.Support.Api.Controller;
    using Totira.Support.Api.Options;
    using Totira.Support.Application.Messages;
    using Totira.Support.EventServiceBus;

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

        //mock
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

        [HttpPost("GetAllProperties")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPropertyDto>> GetAllProperties(QueryProperty query)
        {

            string serializedQuery = JsonConvert.SerializeObject(query.QueryFilter);
            string encodedQuery = HttpUtility.UrlEncode(serializedQuery);
            var PropertyData = await _queryRestClient.GetAsync<GetPropertyDto>($"{_restClientOptions.Properties}/property/getallproperties/{query.SortBy}/{query.PageNumber}/{query.PageSize}/{encodedQuery}");
            if (PropertyData == null)
                return NotFound();

            return PropertyData;
        }


        [HttpPost("CreatePropertyfromRETS")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePropertyfromMLS([FromBody] CreatePropertyfromRETSCommand command)
        {
            string url = Url.Action("Get", "Property", new { propertyType = command.PropertyType, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url,Guid.Empty);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }



        [HttpGet("Property/carrousel/{ml_num}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPropertyCarrouselImagesDto>> GetPropertyCarrouselImage(string ml_num)
        {

            GetPropertyCarrouselImagesDto mock = new GetPropertyCarrouselImagesDto();
            mock.CarrouselImages = new List<PropertyImage>();
            var images = new List<PropertyImage>(); ;

            images.Add(new PropertyImage() { Url = "https://s3.amazonaws.com/imagenesprof.fincaraiz.com.co/OVFR_COL/2022/10/12/78738354.jpg" });
            images.Add(new PropertyImage() { Url = "https://s3.amazonaws.com/imagenesprof.fincaraiz.com.co/OVFR_COL/2022/10/27/79478057.jpg" });
            images.Add(new PropertyImage() { Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRDz_CCuuFhUNG9oLKxLQU88gqcJ_M0zHkBWg&usqp=CAU" });
            images.Add(new PropertyImage() { Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSS1F6Tde0HWQws7upDD-d1L0SdKpKXHRdHpg&usqp=CAU" });
            images.Add(new PropertyImage() { Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQUEWzl_jYTj8UvTlPscxTqqhCgYH18TCpiqg&usqp=CAU" });
            images.Add(new PropertyImage() { Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRjkinDp_USguncFcCNfwktVCthEsp8NkYHQw&usqp=CAU" });
            images.Add(new PropertyImage() { Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSkVQT88x_48gEEsx5T1SqbX6AOcnrR0g1qBw&usqp=CAU" });
            images.Add(new PropertyImage() { Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTYCMICLKxu1wNdQSm3QKT41jLkaAcmlfb2r7nDttjEZHgQu3bJKGl1jbJ3KXxK1TnlnEA&usqp=CAU" });
            images.Add(new PropertyImage() { Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTq44s0qyrTQ6lFTU-xjGHs_8JefhM8YEnVig&usqp=CAU" });

            mock.CarrouselImages = images;
            return mock;


        }



        [HttpPost("ImportPropertyImagesToS3")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ImportPropertyImagesToS3([FromBody] ImportPropertyImagesToS3Command command)
        {

            string url = Url.Action("Get", "ImportPropertyImagesToS3", new { propertyId = command.PropertyId, version = API_VERSION }) ?? string.Empty;
            var context = _contextFactory.Create(url,Guid.Empty);
            await _bus.PublishAsync(context, command);
            return Accepted(context);
        }


    }
}

