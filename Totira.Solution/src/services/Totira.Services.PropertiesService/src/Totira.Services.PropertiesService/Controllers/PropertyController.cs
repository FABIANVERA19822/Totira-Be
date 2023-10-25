using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;

namespace Totira.Services.PropertiesService.Controllers;

public class PropertyController : DefaultBaseController
{
    private readonly IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto> _getypropertydataByIdHandler;
    private readonly IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>> _getLocationsBySearchKeword;
    private readonly IQueryHandler<QueryProperty, GetPropertyDto> _getypropertydataHandler;
    private readonly IQueryHandler<QueryPropertyDetailsToApply, GetPropertyDetailstoApplyDto> _getpropertyDetailsdataHandler;
    private readonly IQueryHandler<QueryPropertyCarrouselImagesByMl_num, GetPropertyCarrouselImagesDto> _getpropertyCarrouselImagesHandler;
    private readonly IQueryHandler<QueryPropertyMap, IEnumerable<GetPropertyMapDto>> _getPropertyMapHandler;
    private readonly IQueryHandler<QueryPropertyApplicationDetail, GetPropertyApplicationDetailDto> _getPropertyApplicationDetailHandler;
    private readonly ILogger<PropertyController> _logger;

    public PropertyController(
        ILogger<PropertyController> logger,
        IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto> getypropertydataByIdHandler,
        IQueryHandler<QueryProperty, GetPropertyDto> getypropertydataHandler,
        IQueryHandler<QueryPropertyDetailsToApply, GetPropertyDetailstoApplyDto> getpropertyDetailsdataHandler,
        IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>> getLocationsBySearchKeword,
        IQueryHandler<QueryPropertyCarrouselImagesByMl_num, GetPropertyCarrouselImagesDto> getpropertyCarrouselImagesHandler,
        IQueryHandler<QueryPropertyMap, IEnumerable<GetPropertyMapDto>> getPropertyMapHandler,
        IQueryHandler<QueryPropertyApplicationDetail, GetPropertyApplicationDetailDto> getPropertyApplicationDetailHandler)
    {
        _getypropertydataByIdHandler = getypropertydataByIdHandler;
        _logger = logger;
        _getypropertydataHandler = getypropertydataHandler;
        _getpropertyDetailsdataHandler = getpropertyDetailsdataHandler;
        _getLocationsBySearchKeword = getLocationsBySearchKeword;
        _getpropertyCarrouselImagesHandler = getpropertyCarrouselImagesHandler;
        _getPropertyMapHandler = getPropertyMapHandler;
        _getPropertyApplicationDetailHandler = getPropertyApplicationDetailHandler;
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

    [HttpGet("list/{sortBy}/{pageNumber}/{pageSize}/{serializedFilter}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetPropertyDto>> List(EnumPropertySortBy sortBy, int pageNumber, int pageSize, string? serializedFilter)
    {
        var filter = string.IsNullOrEmpty(serializedFilter)
            ? default
            : JsonConvert.DeserializeObject<QueryFilter>(serializedFilter);

        if (!ModelState.IsValid) return BadRequest();

        return await _getypropertydataHandler.HandleAsync(new QueryProperty(sortBy, pageNumber, pageSize, filter));
    }

    [HttpGet("map/{sortBy}/{pageNumber}/{pageSize}/{serializedFilter}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<GetPropertyMapDto>>> GetMap(EnumPropertySortBy sortBy, int pageNumber, int pageSize, string? serializedFilter)
    {
        var filter = string.IsNullOrEmpty(serializedFilter)
            ? default
            : JsonConvert.DeserializeObject<QueryFilter>(serializedFilter);

        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(await _getPropertyMapHandler.HandleAsync(new QueryPropertyMap(sortBy, pageNumber, pageSize, filter)));
    }

    [HttpGet("propertyDetails/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetPropertyDetailstoApplyDto>> GetPropertyDetailstoApply(string propertyId)
    {
        if (!ModelState.IsValid) return BadRequest();

        return await _getpropertyDetailsdataHandler.HandleAsync(new QueryPropertyDetailsToApply(propertyId));
    }

    [HttpGet("getPropertyCarrouselImage/{ml_num}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetPropertyCarrouselImagesDto>> GetPropertyCarrouselImage(string ml_num)
    {
        if (!ModelState.IsValid) return BadRequest();

        return await _getpropertyCarrouselImagesHandler.HandleAsync(new QueryPropertyCarrouselImagesByMl_num(ml_num));
    }

    [HttpGet("{propertyId}/detail/application-request")]
    [ProducesResponseType(typeof(GetPropertyApplicationDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetPropertyApplicationDetailDto>> GetPropertyApplicationDetail(string propertyId)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return await _getPropertyApplicationDetailHandler.HandleAsync(new QueryPropertyApplicationDetail(propertyId));
    }
}
