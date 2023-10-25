namespace Totira.Services.RootService.DTO
{
    public class GetTenantApplicationTypeByDto
    {
        public Guid TenantId { get; set; }
        public string ApplicationType { get; set; } = string.Empty;
    }
}
