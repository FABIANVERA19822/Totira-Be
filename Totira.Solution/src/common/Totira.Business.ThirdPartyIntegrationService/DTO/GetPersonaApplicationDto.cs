namespace Totira.Business.ThirdPartyIntegrationService.DTO
{
    public class GetPersonaApplicationDto
    {
        public string InquiryId { get; set; }
        public Guid TenantId { get; set; }
        public List<string> UrlImages { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
