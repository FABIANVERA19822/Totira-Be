namespace Totira.Services.RootService.DTO
{
    public class GetPropertyCarrouselImagesDto
    {
        public List<PropertyImage> CarrouselImages { get; set; }
        
    }

    public class PropertyImage
    {
        public string Url { get; set; } = string.Empty;
    }
}
