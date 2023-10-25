namespace Totira.Services.RootService.DTO;

using static System.Runtime.InteropServices.JavaScript.JSType;

public class QueryProperty
{
    public EnumPropertySortBy? SortBy { get; set; } = EnumPropertySortBy.Newest;

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public QueryFilter? QueryFilter { get; set; } = new QueryFilter();

    public QueryProperty()
    {

    }
    public QueryProperty(EnumPropertySortBy? sortBy, int pageNumber, int pageSize, QueryFilter? queryFilter = default)
    {
        this.SortBy = sortBy;
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
    }
}

public record QueryFilter
{
    public string? Type { get; set; }
    public decimal? MinimumPrice { get; set; }
    public decimal? MaximumPrice { get; set; }
    public decimal? Bedrooms { get; set; }
    public decimal? Bathrooms { get; set; }
    public decimal? MinimumSize { get; set; }
    public decimal? MaximumSize { get; set; }
    public decimal? Parking { get; set; }
    public string? PetsAllowed { get; set; }
    //public DateTime? Availability { get; set; }
    //public string? ImmediateAvailable { get; set; }
    //Amenities
    public string? Loundry { get; set; }
    public string? Fireplace { get; set; }
    public string? GymFitnessCenter { get; set; }
    public string? StorageSpace { get; set; }
    public string? Balcony { get; set; }
    public string? OnSiteManagement { get; set; }
    public string? Pool { get; set; }
    public string? BikeStorage { get; set; }
    public string? Elevator { get; set; }
    public string? SmokeFreeBuilding { get; set; }
    public string? SecuritySystem { get; set; }
    public string? Furnished { get; set; }
    public string? OutdoorSpace { get; set; }
    public string? Dishwasher { get; set; }


    public string? HeatIncluded { get; set; }
    public string? HydroIncluded { get; set; }
    public string? WaterIncluded { get; set; }
    public string? CableIncluded { get; set; }
    public string? CentralVacuum { get; set; }

    public string? AirConditioning { get; set; }
    public string? Laundry { get; set; }
    public string? Unfurnished { get; set; }
    public string? WheelchairAccesible { get; set; }
    public string? SecurityGuard { get; set; }
    public string? CentralAirConditioning { get; set; }

    public string? RecreationMeetingRoom { get; set; }
    public QueryFilter() { }
}

