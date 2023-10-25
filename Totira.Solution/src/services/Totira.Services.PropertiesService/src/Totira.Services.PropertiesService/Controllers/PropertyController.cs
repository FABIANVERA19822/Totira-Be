namespace Totira.Services.PropertiesService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Totira.Bussiness.PropertiesService.DTO;
    using Totira.Bussiness.PropertiesService.Queries;
    using Totira.Support.Api.Controller;
    using Totira.Support.Application.Queries;

    public class PropertyController : DefaultBaseController
    {
        private readonly IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto> _getypropertydataByIdHandler;
        private readonly IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>> _getLocationsBySearchKeword;
        private readonly IQueryHandler<QueryProperty, GetPropertyDto> _getypropertydataHandler;

        private readonly ILogger<PropertyController> _logger;

        public PropertyController(
            ILogger<PropertyController> logger,
            IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto> getypropertydataByIdHandler,
            IQueryHandler<QueryProperty, GetPropertyDto> getypropertydataHandler,
            IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>> getLocationsBySearchKeword

         )
        {
            _getypropertydataByIdHandler = getypropertydataByIdHandler;
            _logger = logger;
            _getypropertydataHandler = getypropertydataHandler;
            _getLocationsBySearchKeword = getLocationsBySearchKeword;
        }

        //example
        [HttpGet("propertydata/{ml_num}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPropertyDetailsDto>> GetProperty(string ml_num)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _getypropertydataByIdHandler.HandleAsync(new QueryPropertyByMlnum(ml_num));
            return Ok(response);
        }

        [HttpGet("GetLocationsBySearchKeyWord/{SearchKeyword}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<LocationDto>>> GetLocationsBySearchKeyword(string SearchKeyword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return await _getLocationsBySearchKeword.HandleAsync(new QueryLocationsBySearchKeyword(SearchKeyword));
        }

        [HttpGet("getallproperties/{SortBy}/{PageNumber}/{PageSize}/{QueryFilter}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPropertyDto>> GetAllProperties(EnumPropertySortBy SortBy, int PageNumber, int PageSize, string? QueryFilter)
        {
            QueryFilter queryFilter = JsonConvert.DeserializeObject<QueryFilter>(QueryFilter);

            if (!ModelState.IsValid) return BadRequest();

            return await _getypropertydataHandler.HandleAsync(new QueryProperty(SortBy, PageNumber, PageSize, queryFilter));
        }
    }
}