namespace Totira.Bussiness.UserService.DTO
{
    public class GetPropertyAppliedDto
    {
        public string PropertyId { get; set; } = default!;
        public Guid ApplicationRequestId { get; set; }
        public Guid ApplicantId { get; set; }
        public Guid PropertyApplicationdId { get; set; }
        public DateTimeOffset SubmissionDate { get; set; }


        public GetPropertyAppliedDto(string propertyId, Guid applicationRequestId, Guid applicantId, Guid propertyApplicationdId, DateTimeOffset submissionDate)
        {
            PropertyId = propertyId;
            ApplicationRequestId = applicationRequestId;
            ApplicantId = applicantId;
            PropertyApplicationdId = propertyApplicationdId;
            SubmissionDate = submissionDate;
        }

        public GetPropertyAppliedDto() { }
    }
}
