using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.ThirdPartyIntegrationService.Commands.VerifiedProfile
{
    [RoutingKey("CreateTenantVerifiedProfileCommand")]
    public class CreateTenantVerifiedProfileCommand : ICommand
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
