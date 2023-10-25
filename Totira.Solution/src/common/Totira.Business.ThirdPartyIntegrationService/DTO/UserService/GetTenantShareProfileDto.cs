using System;
namespace Totira.Business.ThirdPartyIntegrationService.DTO.UserService
{
	public class GetTenantShareProfileDto
	{
        public Guid TenantId { get; set; }

        public string Email { get; set; } = string.Empty;
        public string TypeOfContact { get; set; } = string.Empty;
        public string PropertyStreetAddress { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string EncryptedAccessCode { get; set; } = string.Empty;
    }
}

