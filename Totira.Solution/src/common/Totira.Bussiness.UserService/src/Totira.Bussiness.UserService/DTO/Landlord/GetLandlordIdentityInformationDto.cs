using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.DTO.Common;

namespace Totira.Bussiness.UserService.DTO.Landlord
{
    public class GetLandlordIdentityInformationDto
    {
        public Guid LandlordId { get; set; }
        public ContactInformationPhoneNumberDto PhoneNumber { get; set; }
        public List<FileInfoDisplayDto> IdentityProofs { get; set; }

        protected GetLandlordIdentityInformationDto(Guid id,
            ContactInformationPhoneNumberDto phoneNumber, 
            List<FileInfoDisplayDto> identityProofs)
        {
            LandlordId = id;
            PhoneNumber = new ContactInformationPhoneNumberDto(phoneNumber.Number,phoneNumber.CountryCode);
            IdentityProofs = identityProofs;
        }
        protected GetLandlordIdentityInformationDto(Guid id)
        {
            LandlordId = id;
            PhoneNumber = null;
            IdentityProofs = null;
        }
        public static GetLandlordIdentityInformationDto Empty(Guid LandlordId) => new(LandlordId);

        public static GetLandlordIdentityInformationDto AdaptFrom(LandlordIdentityInformation entity)
            => new(entity.LandlordId,
                   ContactInformationPhoneNumberDto.AdaptFrom(entity.PhoneNumber),
                   FileInfoDisplayDto.AdaptFrom(entity.IdentityProofs));
    }

}
