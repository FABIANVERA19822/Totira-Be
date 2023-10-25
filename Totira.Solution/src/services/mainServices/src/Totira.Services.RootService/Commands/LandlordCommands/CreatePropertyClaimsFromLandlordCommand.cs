using System.ComponentModel.DataAnnotations;
using Totira.Services.RootService.DTO.Common;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands.LandlordCommands
{
    [RoutingKey("CreatePropertyClaimsFromLandlordCommand")]
    public class CreatePropertyClaimsFromLandlordCommand : ICommand
    {
        public Guid LandlordId { get; set; }
        public string Role { get; set; }
        public List<PropertyClaimDetailDto> ClaimDetails { get; set; }
    }
}
