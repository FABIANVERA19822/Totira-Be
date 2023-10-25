using Totira.Services.RootService.DTO.Common;

namespace Totira.Services.RootService.Commands.LandlordCommands
{
    public class PropertyClaimDetailDto
    {
        public string MlsID { get; set; }
        public string Address { get; set; }
        public string Unit { get; set; }
        public string City { get; set; }
        public string ListingUrl { get; set; }
        public List<FileInfoDto> OwnershipProofs { get; set; }
    }
}
