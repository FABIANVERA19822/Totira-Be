namespace Totira.Bussiness.UserService.DTO
{
    public class GetTenantAquaintanceReferralEmailsDto
    {
        public Guid TenantId { get; set; }
        public List<AquaintanceReferralEmailDto>? AquaintanceReferralEmails { get; set; }
    }
    public class AquaintanceReferralEmailDto
    {
        public AquaintanceReferralEmailDto(string email)
        {
            Email = email;
        }
        public string Email { get; set; } = String.Empty;
    }
}

