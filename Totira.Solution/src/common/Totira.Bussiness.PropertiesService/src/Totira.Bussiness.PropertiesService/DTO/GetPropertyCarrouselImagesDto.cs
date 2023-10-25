namespace Totira.Bussiness.PropertiesService.DTO
{
    public class GetPropertyCarrouselImagesDto
    {
        public List<PropertyImage> CarrouselImages { get; set; } = new List<PropertyImage>();

    }

    public class PropertyImage
    {
        public string Url { get; set; } = string.Empty;
    }
}
