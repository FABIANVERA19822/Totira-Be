using System;
namespace Totira.Services.RootService.DTO
{
	public class GetTenantApplicationListPageDto
	{
        public List<TenantSingleApplication> ApplicationListPage { get; set; }
    }
    public class TenantSingleApplication
    {
        public string PropertyId { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public PropertyMainImage PropertyImageFile { get; set; } = default!;
        public string Area { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal ListPrice { get; set; }

        public GetTenantApplicationDetailsDto ApplicationDetails { get; set; }

        public List<CoApplicantsImageAndName> CoApplicantsInfo { get; set; }


    }
    public class CoApplicantsImageAndName
    {
        public string Role { get; set; }
        public string CoApplicantName { get; set; }
        public GetTenantProfileImageDto CoApplicantsImage { get; set; }
    }
    public class PropertyMainImage
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = default!;
        public string FileUrl { get; set; } = default!;

        public PropertyMainImage(string fileName, string contentType, string fileUrl)
        {
            FileName = fileName;
            ContentType = contentType;
            FileUrl = fileUrl;
        }
    }
}

