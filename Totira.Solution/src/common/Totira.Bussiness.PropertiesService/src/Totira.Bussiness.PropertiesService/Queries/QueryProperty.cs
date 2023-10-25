
namespace Totira.Bussiness.PropertiesService.Queries;

using Totira.Support.Application.Queries;

public class QueryProperty : IQuery
{
    public EnumPropertySortBy? SortBy { get; set; } = EnumPropertySortBy.Newest;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public QueryFilter? Filter { get; set; }

    public QueryProperty(EnumPropertySortBy? sortBy, int pageNumber, int pageSize, QueryFilter? queryFilter)
    {
        SortBy = sortBy;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Filter = queryFilter;
    }
}

public enum EnumPropertySortBy
{
    Newest = 1,
    PriceLowToHigh,
    PriceHighToLow
}

public record QueryFilter
{
    public decimal? MinimumPrice { get; set; }
    public decimal? MaximumPrice { get; set; }
    public string? Type { get; set; }
    public double? MinimumSize { get; set; }
    public double? MaximumSize { get; set; }
    public string? Bedrooms { get; set; }
    public string? Bathrooms { get; set; }
    public QueryAmenities? Amenities { get; set; }
    public QueryUtilities? Utilities { get; set; }
    public string? Parking { get; set; }
    public bool PetsAllowed { get; set; } 
    public string? Area { get; set; } 
    public string? Province { get; set; }

}

public class QueryAmenities
{
    public bool AirConditioning { get; set; }
    public bool Laundry { get; set; }
    public bool Pool { get; set; }
    public bool Furnished { get; set; }
    public bool Unfurnished { get; set; }
    public bool GymOrFitnessCenter { get; set; }
    public bool Balcony { get; set; }
    public bool WheelchairAccesible { get; set; }
    public bool SecurityGuardOrConcierge { get; set; }
    public bool SecuritySystem { get; set; }
    public bool OutdoorSpace { get; set; }
    public bool RecreationOrMeetingRoom { get; set; }
    public bool CentralVacuum { get; set; }
}

public class QueryUtilities
{
    public bool HeatIncluded { get; set; }
    public bool HydroIncluded { get; set; }
    public bool WaterIncluded { get; set; }
    public bool CableIncluded { get; set; }
    public bool CentralAirConditioning { get; set; }
}