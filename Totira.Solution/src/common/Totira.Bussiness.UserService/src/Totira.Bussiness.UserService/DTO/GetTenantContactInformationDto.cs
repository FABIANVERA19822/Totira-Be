using Totira.Bussiness.UserService.DTO.Common;

namespace Totira.Bussiness.UserService.DTO
{
    public class GetTenantContactInformationDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string HousingStatus { get; set; }
        public string SelectedCountry { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string StreetAddress { get; set; }
        public string Unit { get; set; }
        public string Email { get; set; }
        public ContactInformationPhoneNumberDto? PhoneNumber { get; set; } = new ContactInformationPhoneNumberDto(string.Empty, string.Empty);
    }

}
