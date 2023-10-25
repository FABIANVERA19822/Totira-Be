

namespace Totira.Business.ThirdPartyIntegrationService.DTO.UserService
{
    using Totira.Business.ThirdPartyIntegrationService.DTO.UserService.Common;
    public class GetPendingLandlordClaimsDto
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string MlsID { get; set; }
        public string Address { get; set; }
        public string Unit { get; set; }
        public string City { get; set; }
        public string ListingUrl { get; set; }
        public List<FileInfoDisplayDto> OwnershipProofs { get; set; }
    }
}