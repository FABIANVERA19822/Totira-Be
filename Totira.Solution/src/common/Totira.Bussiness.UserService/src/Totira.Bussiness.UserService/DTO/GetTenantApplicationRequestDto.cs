namespace Totira.Bussiness.UserService.DTO
{
    public class GetTenantApplicationRequestDto
    {
        public Guid ApplicationId { get; set; }
        public Guid? ApplicationDetailsId { get; set; }
        public Guid TenantId { get; set; }
        public bool Owner { get; set; }
        public bool IsMulti { get; set; }
        public List <CoApplicantDto>? Coapplicants { get; set; }
        public CoApplicantDto? Guarantor { get; set; }
        public bool InValidationProcess { get; set; }
        public DateTimeOffset CreatedOn { get; set; }



    }

    public class CoApplicantDto {
        public Guid? CoapplicantId { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }

    }
}
