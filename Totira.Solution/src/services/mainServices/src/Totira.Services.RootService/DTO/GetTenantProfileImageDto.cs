
namespace Totira.Services.RootService.DTO
{
    public class GetTenantProfileImageDto : GetTenantDto
    {
        public ProfileImageFile Filename { get; set; } = default!;
        public GetTenantProfileImageDto() { }


        public class ProfileImageFile
        {
            public string FileName { get; set; } = string.Empty;
            public string ContentType { get; set; } = default!;
            public string FileUrl { get; set; } = default!;

            public ProfileImageFile(string fileName, string contentType, string fileUrl)
            {
                FileName = fileName;
                ContentType = contentType;
                FileUrl = fileUrl;
            }
        }
    }
}


