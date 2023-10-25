using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("UpdateTenantAcquaintanceReferralCommand")]
    public class UpdateTenantAcquaintanceReferralCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string OtherRelationship { get; set; } = string.Empty;
        public ContactInformationPhoneNumber Phone { get; set; } = new ContactInformationPhoneNumber(string.Empty, string.Empty);
        public string Status { get; set; } = string.Empty;

    }
}

