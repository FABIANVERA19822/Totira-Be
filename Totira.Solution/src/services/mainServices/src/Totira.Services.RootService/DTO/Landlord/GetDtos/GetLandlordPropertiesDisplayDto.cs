namespace Totira.Services.RootService.DTO.Landlord.GetDtos
{
    public class GetLandlordPropertiesDisplayDto
    {
        public long ClaimedCount { get; set; } = 0;
        public long PublishedCount { get; set; } = 0;
        public long UnpublishedCount { get; set; } = 0;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<PropertyDisplayDto> ListedProperties { get; set; } = new();
        public List<PropertyDisplayDto> UnpublishedProperties { get; set; } = new();
    }
}
