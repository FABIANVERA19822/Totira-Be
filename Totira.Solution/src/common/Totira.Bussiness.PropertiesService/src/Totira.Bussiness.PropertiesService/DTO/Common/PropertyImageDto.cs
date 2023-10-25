namespace Totira.Bussiness.PropertiesService.DTO.Common;

public class PropertyImageDto
{
    public PropertyImageDto()
    {
    }

    public PropertyImageDto(string? fileUrl, string? contentType)
    {
        FileUrl = fileUrl;
        ContentType = contentType;
    }
    

    public string? FileUrl { get; set; }
    public string? ContentType { get; set; }
}