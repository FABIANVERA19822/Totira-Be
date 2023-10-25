using Totira.Bussiness.UserService.DTO.Common;

namespace Totira.Bussiness.UserService.DTO.Landlord
{
    public class PropertyClaimDetailsDto
    {
        public string MlsID { get; set; }
        public string Address { get; set; }
        public string Unit { get; set; }
        public string City { get; set; }
        public string ListingUrl { get; set; }
        public List<FileInfoDto> OwnershipProofs { get; set; }
    }
}
