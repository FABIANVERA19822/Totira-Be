using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totira.Services.RootService.DTO.Common
{
    public class FileInfoDisplayDto
    {
        public string FileName { get; set; } = default!;
        public long Size { get; set; }
        public string ContentType { get; set; } = default!;
    }
}
