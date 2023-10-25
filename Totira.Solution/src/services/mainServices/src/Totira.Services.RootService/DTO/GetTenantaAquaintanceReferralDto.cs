namespace Totira.Services.RootService.DTO
{
    public class GetTenantaAquaintanceReferralDto
    {
        public Guid TenantId { get; set; }
        public List<AquaintanceReferralDto>? AquaintanceReferrals { get; set; }
    }

    public class AquaintanceReferralDto
    {
        public AquaintanceReferralDto(Guid referralid, string fullname, string email, string relationship, string phone, string status)
        {
            Referralid = referralid;
            FullName = fullname;
            Email = email;
            Relationship = relationship;
            Phone = phone;
            Status = status;
        }
        public Guid Referralid { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

    }
}
