namespace Totira.Bussiness.UserService.DTO
{
    public class GetTenantApplicationTypeByDto
    {
        public Guid TenantId { get; set; }
        public string ApplicationType { get; set; } = string.Empty;



    }
}
