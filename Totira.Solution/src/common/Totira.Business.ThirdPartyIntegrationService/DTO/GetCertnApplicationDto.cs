namespace Totira.Business.ThirdPartyIntegrationService.DTO
{
    public class GetCertnApplicationDto
    {
        public bool RequestIdentityVerification { get; set; }
        public bool RequestEquifax { get; set; }
        public bool RequestBase { get; set; }
        public bool RequestSoftcheck { get; set; }
        public string Id { get; set; }
        public string Created { get; set; } = string.Empty;
        public string Modified { get; set; } = string.Empty;
        public string LastUpdated { get; set; } = string.Empty;
        public string SubmittedTime { get; set; } = string.Empty;
        public bool IsSubmitted { get; set; }
        public string ApplicantType { get; set; } = string.Empty;
        public string StatusSoftCheck { get; set; } = string.Empty;
        public string StatusEquifax { get; set; } = string.Empty;
        public string JsonResponse { get; set; } = string.Empty;
        public GetCertnApplicationDto(string id, string statusSoftCheck, string statusEquifax, string jsonResponse)
        {
            Id = id;
            StatusEquifax = statusEquifax;
            StatusSoftCheck = statusSoftCheck;
            JsonResponse = jsonResponse;
        }

    }
}
