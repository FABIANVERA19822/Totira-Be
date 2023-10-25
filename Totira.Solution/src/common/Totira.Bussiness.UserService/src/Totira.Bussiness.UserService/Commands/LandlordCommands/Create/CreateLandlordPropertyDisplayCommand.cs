using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands.LandlordCommands.Create
{
    [RoutingKey("CreateLandlordPropertyDisplayCommand")]
    public class CreateLandlordPropertyDisplayCommand : ICommand
    {
        public Guid ClaimId { get; set; }
        public string MLSId { get; set; }
    }
}
