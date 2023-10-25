namespace Totira.Services.Thirdparty.Worker.Outbox.Bll.DTO
{
    public class ListTenantVerifiedProfileDto
    {
        public int Count { get; set; } = 0;
        public List<TenantVerifiedProfileDto> VerifiedProfiles { get; set; } = new List<TenantVerifiedProfileDto>();
    }
}
