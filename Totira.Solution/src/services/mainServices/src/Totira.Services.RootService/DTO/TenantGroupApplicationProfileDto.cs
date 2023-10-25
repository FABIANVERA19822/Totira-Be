namespace Totira.Services.RootService.DTO
{
    public class TenantGroupApplicationProfileDto
    {
        public Guid TenantId { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int InvinteeType { get; set; }
        public int Status { get; set; } = 1;
    }
}
