namespace Totira.Services.RootService.DTO
{
    public class GetTenantFeedbackViaLandlordDto
    {
        public int StarScore { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public string PropertyAddress { get; set; } = string.Empty;
        public string RentalPeriod { get; set; } = string.Empty;
        public string PhotoLink { get; set; } = string.Empty;
        public string DateCompletation { get; set; } = string.Empty;
    }
}
