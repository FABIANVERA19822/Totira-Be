
namespace Totira.Services.Thirdparty.Worker.Outbox.Bll.DTO
{
    public class ListTenantGroupApplicationProfileDto
    {
        public int Count { get; set; } = 0;
        public List<TenantGroupApplicationProfileDto> VerifiedProfiles { get; set; } = new List<TenantGroupApplicationProfileDto>();
    }
}
