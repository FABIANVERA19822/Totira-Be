namespace Totira.Bussiness.UserService.Domain.Common
{
    public class ContactInformationPhoneNumber
    {
        public ContactInformationPhoneNumber(string phoneNumper, string countryCode)
        {
            Number = phoneNumper;
            CountryCode = countryCode;
        }
        public string Number { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }
}
