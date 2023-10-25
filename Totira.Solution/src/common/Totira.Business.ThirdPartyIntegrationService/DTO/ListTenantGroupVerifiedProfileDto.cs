
namespace Totira.Business.ThirdPartyIntegrationService.DTO
{
    public class ListTenantGroupVerifiedProfileDto
    {
        public int Count { get; set; } = 0;
        public List<TenantGroupVerifiedProfileDto> GroupVerifiedProfiles { get; set; } = new List<TenantGroupVerifiedProfileDto>();
    }
}
