namespace Totira.Bussiness.UserService.DTO
{

    public class GetTenantProfileImageDto
    {
        public Guid Id { get; set; }
        public ProfileImageFile Filename { get; set; } = default!;

        public GetTenantProfileImageDto(Guid id, ProfileImageFile filename)
        {
            Id = id;
            Filename = filename;
        }

        public class ProfileImageFile
        {
            public string FileName { get; set; } = string.Empty;
            public string ContentType { get; set; } = default!;
            public string? FileUrl { get; set; } = default!;

            public ProfileImageFile(string fileName, string contentType, string fileUrl)
            {
                FileName = fileName;
                ContentType = contentType;
                FileUrl = fileUrl;
            }
        }
    }
}
