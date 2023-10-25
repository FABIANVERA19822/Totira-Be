namespace Totira.Business.ThirdPartyIntegrationService.DTO.UserService.Common
{

    public class TenantFileDisplayDto
    {
        public string FileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public long Size { get; set; }
    }
}