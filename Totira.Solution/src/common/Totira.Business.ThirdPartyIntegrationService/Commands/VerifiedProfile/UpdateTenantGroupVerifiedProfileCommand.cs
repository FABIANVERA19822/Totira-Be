
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Commands.VerifiedProfile
{
    [RoutingKey("UpdateTenantGroupVerifiedProfileCommand")]
    public class UpdateTenantGroupVerifiedProfileCommand : ICommand
    {
        public Guid TenantId { get; set; }

        public bool? Certn { get; set; }

        public bool? Jira { get; set; }

        public bool? Persona { get; set; }

        public bool IsVerifiedEmailConfirmation { get; set; }
    }
}
