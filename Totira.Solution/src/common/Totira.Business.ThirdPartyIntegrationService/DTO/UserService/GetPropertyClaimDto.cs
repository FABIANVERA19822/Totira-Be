

using Totira.Business.ThirdPartyIntegrationService.DTO.UserService.Common;

namespace Totira.Business.ThirdPartyIntegrationService.DTO.UserService
{
    
    public class GetPropertyClaimDto
    {
        public Guid LandlordId { get; set; }
        public string Role { get; set; }
        public string MlsID { get; set; }
        public string Address { get; set; }
        public string Unit { get; set; }
        public string City { get; set; }
        public string ListingUrl { get; set; }
        public List<LandlordFileInfoDto> OwnershipProofs { get; set; }
    }
}