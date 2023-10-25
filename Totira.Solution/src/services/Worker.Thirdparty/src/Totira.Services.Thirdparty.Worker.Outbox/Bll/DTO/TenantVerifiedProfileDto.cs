namespace Totira.Services.Thirdparty.Worker.Outbox.Bll.DTO
{
    public class TenantVerifiedProfileDto
    {
        public TenantVerifiedProfileDto() { }
        public TenantVerifiedProfileDto(
           string id,
           Guid tenantId,
           bool certn,
           bool jira,
           bool persona,
           bool isVerifiedEmailConfirmtion,
           DateTimeOffset createdOn)
        {
            Id = id;
            TenantId = tenantId;
            Certn = certn;
            Jira = jira;
            Persona = persona;
            IsVerifiedEmailConfirmation = isVerifiedEmailConfirmtion;
            CreatedOn = createdOn;
        }
        public string Id { get; set; }
        public Guid TenantId { get; set; }
        public bool Certn { get; set; }
        public bool Jira { get; set; }
        public bool Persona { get; set; }
        public bool IsVerifiedEmailConfirmation { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
