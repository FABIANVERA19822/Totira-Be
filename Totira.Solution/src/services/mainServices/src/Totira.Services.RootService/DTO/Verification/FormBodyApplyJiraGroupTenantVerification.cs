using System.ComponentModel.DataAnnotations;

namespace Totira.Services.RootService.DTO.Verification;

public class FormBodyApplyJiraGroupTenantVerification
{
    [Required]
    public Guid MainTenantId { get; set; }
    [Required]
    public bool IsReVerificationRequested { get; set; }
}