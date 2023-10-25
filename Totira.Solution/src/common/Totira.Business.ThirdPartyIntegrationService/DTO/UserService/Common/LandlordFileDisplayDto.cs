using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totira.Business.ThirdPartyIntegrationService.DTO.UserService.Common
{
    public class LandlordFileDisplayDto
    {
        public string FileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public long Size { get; set; }
    }
}
