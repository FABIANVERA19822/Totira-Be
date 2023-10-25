namespace Totira.Services.Thirdparty.Worker.Outbox.Bll.DTO
{
    public class GetTenantVerifiedProfileDto
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public bool Certn { get; set; }
        public bool Jira { get; set; }
        public bool Persona { get; set; }
        public bool IsVerifiedEmailConfirmation { get; set; }
        public GetTenantVerifiedProfileDto(string id, string tenantId, bool certn, bool jira, bool persona, bool isVerifiedEmailConfirmation)
        {
            Id = id;
            TenantId = tenantId;
            Certn = certn;
            Jira = jira;
            Persona = persona;
            IsVerifiedEmailConfirmation = isVerifiedEmailConfirmation;
        }

        public GetTenantVerifiedProfileDto()
        {
        }
    }
}
