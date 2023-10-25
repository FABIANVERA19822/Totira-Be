namespace Totira.Services.RootService.DTO
{
    public class UpdateTenantProfileImageDto
    {
        public Guid TenantId { get; set; }
        public IFormFile? File { get; set; } = default;
    }
}
