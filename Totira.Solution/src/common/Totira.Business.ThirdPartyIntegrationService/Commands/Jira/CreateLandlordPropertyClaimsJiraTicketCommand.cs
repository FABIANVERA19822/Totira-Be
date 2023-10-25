using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Commands.Jira
{
    [RoutingKey("CreateLandlordPropertyClaimsJiraTicketCommand")]
    public class CreateLandlordPropertyClaimsJiraTicketCommand : ICommand
    {
        public Guid LandlordId { get; set; }
    }
}