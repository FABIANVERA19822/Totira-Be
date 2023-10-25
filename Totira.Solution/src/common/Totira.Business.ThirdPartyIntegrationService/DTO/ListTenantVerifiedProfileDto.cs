namespace Totira.Business.ThirdPartyIntegrationService.DTO
{
    public class ListTenantVerifiedProfileDto
    {
        public int Count { get; set; } = 0;
        public List<TenantVerifiedProfileDto> VerifiedProfiles { get; set; } = new List<TenantVerifiedProfileDto>();
    }
}
