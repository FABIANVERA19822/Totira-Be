using Totira.Bussiness.PropertiesService.DTO.Common;

namespace Totira.Bussiness.PropertiesService.DTO;

public class GetPropertyMapDto
{
    public string? Id { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public GetPropertyMapInfoDto? Info { get; set; }
}

public class GetPropertyMapInfoDto
{
    public string? Area { get; set; }
    public PropertyImageDto? Photo { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public string? ApproxSquareFootage { get; set; }
    public decimal Price { get; set; }
    public string? StreetName { get; set; }
}