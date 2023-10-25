using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands.LandlordCommands
{
    [RoutingKey("CreateLandlordBasicInfoCommand")]
    public class CreateLandlordBasicInfoCommand: ICommand
    {
        public Guid LandlordId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? SocialInsuranceNumber { get; set; }
        public BasicInformationBirthday? Birthday { get; set; }
        public string AboutMe { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
