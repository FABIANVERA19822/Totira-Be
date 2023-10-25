namespace Totira.Business.ThirdPartyIntegrationService.DTO
{
    public class ListTenantApplicationDto 
    {
        public int Count { get; set; } = 0;
        public List<TenantApplicationDto> TenantApplications { get; set; } = new List<TenantApplicationDto>();
    }
}
