
namespace Totira.Bussiness.UserService.DTO
{
    public class GetTenantCurrentJobStatusByDto
    {
        public Guid TenantId { get; set; }
        public string CurrentJobStatus { get; set; } = string.Empty;
        public bool IsUnderRevisionSend { get; set; }
    }
}
