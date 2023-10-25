using Microsoft.AspNetCore.Mvc;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries;
using Totira.Support.Api.Controller;
using Totira.Support.Application.Queries;

namespace Totira.Services.ThirdPartyIntegrationService.Controllers
{
  
    public class LocationController : DefaultBaseController
    {
        private readonly IQueryHandler<QueryPropertyLocationByAddress, GetPropertyLongitudeAndLatitudeDto> _locationdataHandler;
        private readonly ILogger<LocationController> _logger;

        public LocationController(IQueryHandler<QueryPropertyLocationByAddress, GetPropertyLongitudeAndLatitudeDto> locationdataHandler,
          ILogger<LocationController> logger)
        {
            _locationdataHandler = locationdataHandler;
            _logger = logger;
        }

        [HttpGet("propertylocation/{address}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPropertyLongitudeAndLatitudeDto>> GetPropertyLocation(string address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _locationdataHandler.HandleAsync(new QueryPropertyLocationByAddress(address));
            return Ok(response);
        }
    }
}

