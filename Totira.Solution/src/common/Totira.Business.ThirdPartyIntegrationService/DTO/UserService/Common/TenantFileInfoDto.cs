using Microsoft.AspNetCore.Http;

namespace Totira.Business.ThirdPartyIntegrationService.DTO.UserService.Common
{
    public class TenantFileInfoDto
    {
        public TenantFileInfoDto(IFormFile file)
        {
            using var ms = new MemoryStream();

            file.CopyTo(ms);

            FileName = file.FileName;
            Length = file.Length;
            ContentType = file.ContentType;
            Data = ms.ToArray();
        }

        public string FileName { get; set; }
        public long Length { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}