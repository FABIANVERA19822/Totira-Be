using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.DTO.Landlord;

namespace Totira.Bussiness.UserService.DTO.Common
{
    public class ContactInformationPhoneNumberDto
    {
        public ContactInformationPhoneNumberDto(string number, string countryCode)
        {
            Number = number;
            CountryCode = countryCode;
        }
        public string Number { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;

        public static ContactInformationPhoneNumberDto? AdaptFrom(ContactInformationPhoneNumber? phoneNumber)
            => phoneNumber is null ? default : new(phoneNumber.Number, phoneNumber.CountryCode);
    }

}
