using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("CreateAcquaintanceReferralFormInfoCommand")]
    public class CreateAcquaintanceReferralFormInfoCommand : ICommand
    {
        public Guid ReferralId { get; set; }
        public Guid TenantId { get; set; }
        public string Feedback { get; set; } = string.Empty;       
        public int StarScore { get; set; }

    }


}
