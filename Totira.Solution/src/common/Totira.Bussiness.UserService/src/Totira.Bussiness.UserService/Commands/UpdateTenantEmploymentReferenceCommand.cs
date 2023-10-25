using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("UpdateTenantEmploymentReferenceCommand")]
    public class UpdateTenantEmploymentReferenceCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public EmploymentReferencePhoneNumber PhoneNumber { get; set; } = new EmploymentReferencePhoneNumber();
    }
}

