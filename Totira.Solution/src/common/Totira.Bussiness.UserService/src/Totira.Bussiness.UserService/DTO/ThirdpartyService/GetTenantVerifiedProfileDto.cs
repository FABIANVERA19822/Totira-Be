namespace Totira.Bussiness.UserService.DTO.ThirdpartyService
{
    public class GetTenantVerifiedProfileDto
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public bool Certn { get; set; }
        public bool Jira { get; set; }
        public bool Persona { get; set; }
        public bool IsVerifiedEmailConfirmation { get; set; }
    }
}
