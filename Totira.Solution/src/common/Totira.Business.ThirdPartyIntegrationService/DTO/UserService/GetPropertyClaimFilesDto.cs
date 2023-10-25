using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totira.Business.ThirdPartyIntegrationService.DTO.UserService
{
    public class GetPropertyClaimFilesDto
    {
        public Guid LandlordId { get; set; }
        public List<File> Files { get; set; }

    }
}
