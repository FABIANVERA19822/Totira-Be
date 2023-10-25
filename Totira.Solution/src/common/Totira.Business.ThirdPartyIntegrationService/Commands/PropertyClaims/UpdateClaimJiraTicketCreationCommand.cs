namespace Totira.Business.ThirdPartyIntegrationService.Commands.PropertyClaims
{
    using System;
    using Totira.Support.Application.Commands;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("UpdateClaimJiraTicketCreationCommand")]
    public class UpdateClaimJiraTicketCreationCommand : ICommand
    {
        public Guid ClaimId { get; set; }

        public UpdateClaimJiraTicketCreationCommand(Guid claimId) => this.ClaimId = claimId;
    }
}