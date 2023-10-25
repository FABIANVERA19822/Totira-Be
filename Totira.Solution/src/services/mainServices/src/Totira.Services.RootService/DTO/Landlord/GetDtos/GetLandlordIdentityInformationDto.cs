using Totira.Services.RootService.DTO.Common;

namespace Totira.Services.RootService.DTO.Landlord.GetDtos
{
    public class GetLandlordIdentityInformationDto
    {
        public Guid LandlordId { get; set; }
        public ContactInformationPhoneNumberDto PhoneNumber { get; set; }
        public List<FileInfoDisplayDto> IdentityProofs { get; set; }
    }
}
