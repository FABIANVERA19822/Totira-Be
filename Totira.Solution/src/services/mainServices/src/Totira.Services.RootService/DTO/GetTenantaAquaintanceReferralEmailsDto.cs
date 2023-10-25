namespace Totira.Services.RootService.DTO
{
    public class GetTenantaAquaintanceReferralEmailsDto
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

