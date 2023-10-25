namespace Totira.Bussiness.UserService.DTO
{
    public class GetAcquaintanceReferralFormInfoDto
    {
        public Guid ReferralId { get; set; }
        public string Status { get; set; } = string.Empty;
        public object Comment { get; set; } = string.Empty;
        public object StarScore { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string OtherRelationship { get; set; } = string.Empty;
        public string PhotoLink { get; set; } = string.Empty;
        public string ReferralName { get; set; } = string.Empty;
    }
}
