using System.ComponentModel.DataAnnotations;

namespace Totira.Services.RootService.DTO
{
    public class TenantGroupApplicationShareProfileDto
    {
        [Required(ErrorMessage = "TenantId is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "InvinteeType is required.")]
        public int InvinteeType { get; set; }
        public int Status { get; set; } = 1;
    }
}
