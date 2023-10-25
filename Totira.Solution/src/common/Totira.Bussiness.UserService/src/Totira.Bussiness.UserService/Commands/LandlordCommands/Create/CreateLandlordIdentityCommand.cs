using Totira.Bussiness.UserService.Commands.Common;
using Totira.Bussiness.UserService.DTO.Common;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands.LandlordCommands.Create
{
    [RoutingKey("CreateLandlordIdentityCommand")]
    public class CreateLandlordIdentityCommand : ICommand
    {
        public Guid LandlordId { get; set; }
        public ContactInformationPhoneNumber PhoneNumber { get; set; }
        public List<FileInfoDto> IdentityProofs { get; set; }
    }
}
