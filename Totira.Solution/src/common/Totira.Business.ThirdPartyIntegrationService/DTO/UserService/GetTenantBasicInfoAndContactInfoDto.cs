using System;
namespace Totira.Business.ThirdPartyIntegrationService.DTO.UserService
{
	public class GetTenantBasicInfoAndContactInfoDto
	{
        public GetTenantBasicInformationDto BasicInformation { get; set; } = new GetTenantBasicInformationDto();
        public GetTenantContactInformationDto ContactInformation { get; set; } = new GetTenantContactInformationDto();
        public string Role { get; set; } = string.Empty;
    }
}

