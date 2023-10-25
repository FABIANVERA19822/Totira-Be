using System;
namespace Totira.Business.ThirdPartyIntegrationService.DTO.UserService
{
	public class GetTenantGroupApplicationSummaryDto
	{
        public GetTenantIndividualApplicationSummaryDto? MainApplicant { get; set; }
        public GetTenantIndividualApplicationSummaryDto? Guarantor { get; set; }
        public List<GetTenantIndividualApplicationSummaryDto>? CoApplicants { get; set; } = new List<GetTenantIndividualApplicationSummaryDto>();
    }
    public class GetTenantIndividualApplicationSummaryDto
    {
        public Guid Id { get; set; }
        public GetTenantProfileImageDto? ProfileImage { get; set; }
        public Dictionary<string, int> ProfileProgress { get; set; }
        public string Name { get; set; }
    }
    public class GetTenantProfileImageDto 
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

