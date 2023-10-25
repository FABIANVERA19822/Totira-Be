namespace Totira.Services.RootService.DTO.Common
{
    public class ContactInformationPhoneNumberDto
    {
        public ContactInformationPhoneNumberDto(string phoneNumper, string countryCode)
        {
            Number = phoneNumper;
            CountryCode = countryCode;
        }
        public string Number { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }

}
