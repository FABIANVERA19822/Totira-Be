using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands.LandlordCommands
{
    [RoutingKey("CreateLandlordPropertyClaimsJiraTicketCommand")]
    public class CreateLandlordPropertyClaimsJiraTicketCommand : ICommand
    {
        public Guid LandlordId { get; set; }
    }
}