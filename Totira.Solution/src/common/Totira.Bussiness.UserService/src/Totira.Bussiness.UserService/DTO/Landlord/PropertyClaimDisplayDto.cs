namespace Totira.Bussiness.UserService.DTO.Landlord
{
    public class PropertyClaimDisplayDto
    {
        public string ClaimDate { get; set; }
        public string DetailsType { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
