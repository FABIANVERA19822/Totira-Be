using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Business.ThirdPartyIntegrationService.DTO.UserService.Common;

namespace Totira.Business.ThirdPartyIntegrationService.DTO.UserService
{
    public class GetLandlordIdentityInformationDto
    {
        public Guid LandlordId { get; set; }
        public ContactInformationPhoneNumberDto PhoneNumber { get; set; }
        public List<LandlordFileDisplayDto> IdentityProofs { get; set; }
    }
}
