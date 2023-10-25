using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{

    [RoutingKey("UpdateTenantAcquaintanceReferralReactivateCommand")]
    public class UpdateTenantAcquaintanceReferralReactivateCommand : ICommand
    {
        public Guid ReferralId { get; set; }

    }
}
