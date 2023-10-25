namespace Totira.Bussiness.UserService.Commands.Common
{
    public class ContactInformationPhoneNumber
    {
        public ContactInformationPhoneNumber(string numper, string countryCode)
        {
            Number = numper;
            CountryCode = countryCode;
        }
        public string Number { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }
}
