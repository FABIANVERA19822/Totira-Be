using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands;

[RoutingKey("UpdateTenantBasicInformationCommand")]
public class UpdateTenantBasicInformationCommand : ICommand
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? SocialInsuranceNumber { get; set; }
    public BasicInformationBirthday Birthday { get; set; } = new();
    public string AboutMe { get; set; } = string.Empty;
}


