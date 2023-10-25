using Totira.Services.RootService.DTO.Common;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands.LandlordCommands
{
    [RoutingKey("CreateLandlordIdentityCommand")]
    public class CreateLandlordIdentityCommand : ICommand
    {
        public Guid LandlordId { get; set; }
        public ContactInformationPhoneNumber PhoneNumber { get; set; }
        public List<FileInfoDto> IdentityProofs { get; set; }
    }
}
