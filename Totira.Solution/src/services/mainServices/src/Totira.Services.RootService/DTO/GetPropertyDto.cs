
namespace Totira.Services.RootService.DTO;

public class PropertyDto
{
    public string Id { get; set; } = string.Empty;
    public string Ml_num { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal Bedrooms { get; set; }
    public decimal Washrooms { get; set; }
    public string ApproxSquareFootage { get; set; } = string.Empty;
    public decimal OriginalPrice { get; set; }
    public decimal ListPrice { get; set; }
    public string StreetName { get; set; } = string.Empty;
    public PropertyDto()
    {
    }
}

public class GetPropertyDto
{
    public int Count { get; set; } = 0;
    public List<PropertyDto> Properties { get; set; } = new List<PropertyDto>();
    public EnumPropertySortBy? SortBy { get; set; } = EnumPropertySortBy.Newest;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

}

public enum EnumPropertySortBy
{
    Newest,
    PriceLowToHigh,
    PriceHighToLow
}