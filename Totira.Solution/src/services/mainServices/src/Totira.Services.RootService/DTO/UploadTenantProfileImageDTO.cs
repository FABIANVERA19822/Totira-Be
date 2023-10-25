
namespace Totira.Services.RootService.DTO
{
    public class UploadTenantProfileImageDto
    {
        public Guid TenantId { get; set; }
        public string File { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageType { get; set; } = string.Empty;
    }
}
