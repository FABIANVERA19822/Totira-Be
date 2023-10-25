using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("CreateTenantBasicInformationCommand")]
    public class CreateTenantBasicInformationCommand : ICommand
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string? SocialInsuranceNumber { get; set; }
        public BasicInformationBirthday? Birthday { get; set; }
        public string? AboutMe { get; set; } = string.Empty;
    }

    public class BasicInformationBirthday
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
