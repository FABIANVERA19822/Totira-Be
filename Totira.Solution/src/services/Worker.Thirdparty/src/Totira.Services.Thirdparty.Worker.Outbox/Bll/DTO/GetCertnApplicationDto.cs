namespace Totira.Services.Thirdparty.Worker.Outbox.Bll.DTO
{
    public class GetCertnApplicationDto
    {
        public string Id { get; set; }
        public string StatusEquifax { get; set; } = string.Empty;
        public string StatusSoftCheck { get; set; } = string.Empty;
        public string JsonResponse { get; set; } = string.Empty;
    }
}