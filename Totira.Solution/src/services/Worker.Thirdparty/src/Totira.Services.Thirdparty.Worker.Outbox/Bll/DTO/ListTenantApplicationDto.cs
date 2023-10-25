namespace Totira.Services.Thirdparty.Worker.Outbox.Bll.DTO
{
    public class ListTenantApplicationDto 
    {
        public int Count { get; set; } = 0;
        public List<TenantApplicationDto> TenantApplications { get; set; } = new List<TenantApplicationDto>();
    }
}
