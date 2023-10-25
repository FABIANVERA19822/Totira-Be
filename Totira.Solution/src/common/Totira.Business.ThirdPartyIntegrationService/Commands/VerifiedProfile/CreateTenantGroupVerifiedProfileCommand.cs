
using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Commands;

namespace Totira.Business.ThirdPartyIntegrationService.Commands.VerifiedProfile
{
    public class CreateTenantGroupVerifiedProfileCommand : ICommand
    {
        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }
        [Required(ErrorMessage = "Certn is required.")]
        public bool Certn { get; set; }
        [Required(ErrorMessage = "Jira is required.")]
        public bool Jira { get; set; }
        [Required(ErrorMessage = "Persona is required.")]
        public bool Persona { get; set; }
        [Required(ErrorMessage = "Verified Email Confirmation is required.")]
        public bool IsVerifiedEmailConfirmation { get; set; }
    }
}
