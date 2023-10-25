namespace Totira.Services.RootService.DTO.Landlord.GetDtos
{
    public class GetLandlordClaimsDisplayDto
    {
        public long ClaimedCount { get; set; } = 0;
        public long PublishedCount { get; set; } = 0;
        public long UnpublishedCount { get; set; } = 0;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<PropertyClaimDisplayDto> ClaimedProperties { get; set; } = new();
    }
}
