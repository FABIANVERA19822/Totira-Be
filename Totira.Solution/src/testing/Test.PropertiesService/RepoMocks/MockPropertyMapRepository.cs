using Totira.Bussiness.PropertiesService.Domain;

namespace Test.PropertiesService.RepoMocks;

public class MockPropertyMapRepository
{
    public static IEnumerable<Property> GetFiveOnlyWithLatitudeAndLongitude()
    {
        var list = new List<Property>()
        {
            new()
            {
                Id = "C5948209",
                Latitude = 43.72718,
                Longitude = -79.35524
            },
            new()
            {
                Id = "C5873325",
                Latitude = 43.66467,
                Longitude = -79.38446
            },
            new()
            {
                Id = "C5881180",
                Latitude = 43.67017,
                Longitude = -79.38705
            },
            new()
            {
                Id = "C5948201",
                Latitude = 43.72718,
                Longitude = -79.35524
            },
            new()
            {
                Id = "C5948201",
                Latitude = 43.72718,
                Longitude = -79.35524
            },
        };

        return list;
    }
}
