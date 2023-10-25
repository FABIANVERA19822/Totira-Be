using System;
using GoogleMaps.LocationServices;
using Microsoft.Extensions.Configuration;
using System.Net;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries;
using Totira.Support.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Totira.Support.CommonLibrary.Configurations;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Queries
{
	public class QueryPropertyLocationByAddressHandler : IQueryHandler<QueryPropertyLocationByAddress, GetPropertyLongitudeAndLatitudeDto>
    {
        private readonly IOptions<GoogleLocationServiceSettings> _configuration;
        public QueryPropertyLocationByAddressHandler(
            IOptions<GoogleLocationServiceSettings> configuration)
        {
            _configuration = configuration;
        }


        public async Task<GetPropertyLongitudeAndLatitudeDto> HandleAsync(QueryPropertyLocationByAddress query)
        {
            //"AIzaSyA9m5L3RVslEKWC5YKGuEHAcDCySXt1f4M"
            var locationService =  new GoogleLocationService(apikey: "AIzaSyA9m5L3RVslEKWC5YKGuEHAcDCySXt1f4M");
            var point = locationService.GetLatLongFromAddress(query.Address);

            var result =
               point != null ?
               new GetPropertyLongitudeAndLatitudeDto(point.Longitude, point.Latitude) : new GetPropertyLongitudeAndLatitudeDto(0,0);

            return result;
        }
    }
}

