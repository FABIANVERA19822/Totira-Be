using Totira.Bussiness.PropertiesService.DTO.Common;

namespace Totira.Bussiness.PropertiesService.DTO;

public class GetPropertyApplicationDetailDto
{
    public string? Area { get; set; }
    public string? Address { get; set; }
    public string? AmountFt2 { get; set; }
    public int AmountBeds { get; set; }
    public int AmountBaths { get; set; }
    public int AmountParkingSpaces { get; set; }
    public string? PropertyFronting { get; set; }
    public bool Pets { get; set; }
    public PropertyImageDto? Image { get; set; }
}