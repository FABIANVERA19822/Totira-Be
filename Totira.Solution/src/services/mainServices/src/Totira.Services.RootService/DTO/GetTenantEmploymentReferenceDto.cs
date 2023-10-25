namespace Totira.Services.RootService.DTO
{
    public class GetTenantEmploymentReferenceDto
    {
        public GetTenantEmploymentReferenceDto(Guid tenantId, string firstName, string lastName, string jobTitle, string relationship, string email, EmploymentReferencePhoneNumber phoneNumber)
        {
            TenantId = tenantId;
            FirstName = firstName;
            LastName = lastName;
            JobTitle = jobTitle;
            Relationship = relationship;
            Email = email;
            PhoneNumber = phoneNumber;

        }
        public Guid TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Relationship { get; set; }
        public string Email { get; set; }
        public EmploymentReferencePhoneNumber PhoneNumber { get; set; }
    }
    public class EmploymentReferencePhoneNumber
    {
        public EmploymentReferencePhoneNumber(string phoneNumper, string countryCode)
        {
            Number = phoneNumper;
            CountryCode = countryCode;
        }
        public string Number { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }

}

