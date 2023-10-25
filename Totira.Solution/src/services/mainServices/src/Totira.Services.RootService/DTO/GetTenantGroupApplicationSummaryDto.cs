using System;
namespace Totira.Services.RootService.DTO
{
	public class GetTenantGroupApplicationSummaryDto
	{
        public Guid? ApplicationRequestId { get; set; }
        public GetTenantIndividualApplicationSummaryDto? MainApplicant { get; set; }
        public GetTenantIndividualApplicationSummaryDto? Guarantor { get; set; }
        public List<GetTenantIndividualApplicationSummaryDto>? CoApplicants { get; set; } = new List<GetTenantIndividualApplicationSummaryDto>();

    }
    public class GetTenantIndividualApplicationSummaryDto
    {
        public Guid ID { get; set; }
        public GetTenantProfileImageDto? ProfileImage { get; set; } 
        public Dictionary<string, int> ProfileProgress { get; set; }
        public string Name { get; set; }
    }
}

