using Totira.Bussiness.UserService.DTO.Landlord;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands.LandlordCommands.Create
{
    [RoutingKey("CreatePropertyClaimsFromLandlordCommand")]
    public class CreatePropertyClaimsFromLandlordCommand : ICommand
    {
        public Guid LandlordId { get; set; }
        public string Role { get; set; }
        public List<PropertyClaimDetailsDto> ClaimDetails { get; set; }
    }
}
