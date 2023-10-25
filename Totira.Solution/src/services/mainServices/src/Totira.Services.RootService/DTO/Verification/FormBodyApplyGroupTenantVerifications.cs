using System.ComponentModel.DataAnnotations;

namespace Totira.Services.RootService.DTO.Verification;

public class FormBodyApplyGroupTenantVerifications
{
    [Required]
    public Guid MainTenantId { get; set; }
    [Required]
    public bool AcceptTermAndConditions { get; set; }
}