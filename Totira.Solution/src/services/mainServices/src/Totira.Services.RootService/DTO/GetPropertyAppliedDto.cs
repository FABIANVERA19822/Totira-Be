namespace Totira.Services.RootService.DTO
{
    public class GetPropertyAppliedDto
    {
        public string PropertyId { get; set; }
        public Guid ApplicationRequestId { get; set; }
        public Guid ApplicantId { get; set; }
        public Guid PropertyApplicationdId { get; set; }
        public DateTimeOffset SubmissionDate { get; set; }

    }
}
