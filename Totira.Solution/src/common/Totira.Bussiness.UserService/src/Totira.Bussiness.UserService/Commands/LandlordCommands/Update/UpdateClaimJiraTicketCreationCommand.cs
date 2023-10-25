using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands.LandlordCommands.Update
{
    [RoutingKey("UpdateClaimJiraTicketCreationCommand")]
    public class UpdateClaimJiraTicketCreationCommand : ICommand
    {
        public Guid ClaimId { get; set; }
        public UpdateClaimJiraTicketCreationCommand(Guid claimId) => this.ClaimId = claimId;
    }
}
