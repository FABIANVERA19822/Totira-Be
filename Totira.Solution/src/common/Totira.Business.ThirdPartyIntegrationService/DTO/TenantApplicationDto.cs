namespace Totira.Business.ThirdPartyIntegrationService.DTO
{
    public class TenantApplicationDto
    {
        public TenantApplicationDto() { }
        public TenantApplicationDto(
            string id,
            Guid applicantId,
            string statusSoftCheck,
            string statusEquifax,
            string response,
            DateTimeOffset createdOn)
        {
            Id = id;
            ApplicantId = applicantId;
            StatusSoftCheck = statusSoftCheck;
            StatusEquifax = statusEquifax;
            Response = response;
            CreatedOn = createdOn;
        }
        public string Id { get; set; }
        public Guid ApplicantId { get; set; }
        public string StatusSoftCheck { get; set; } = default!;
        public string StatusEquifax { get; set; } = default!;
        public string Response { get; set; } = default!;
        public DateTimeOffset CreatedOn { get; set; }

    }
}
