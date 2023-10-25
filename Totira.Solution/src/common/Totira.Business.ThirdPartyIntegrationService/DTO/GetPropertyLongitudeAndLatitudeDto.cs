using System;
namespace Totira.Business.ThirdPartyIntegrationService.DTO
{
	public class GetPropertyLongitudeAndLatitudeDto
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public GetPropertyLongitudeAndLatitudeDto(double longitude , double latitude)
		{
			Longitude = longitude;
			Latitude = latitude;

        }
	}
}

