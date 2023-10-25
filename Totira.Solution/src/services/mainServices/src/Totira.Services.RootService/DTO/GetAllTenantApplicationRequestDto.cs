namespace Totira.Services.RootService.DTO
{
    public class GetAllTenantApplicationRequestDto
    {
        public Guid TenantId { get; set; }
        public List<GetTenantApplicationRequestDto> Applications { get; set; }

        public Guid CurrentActive { get; set; }
    }
}
