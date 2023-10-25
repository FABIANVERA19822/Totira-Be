using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    
        [RoutingKey("AcceptTermsAndConditionsCommand")]
        public class AcceptTermsAndConditionsCommand : ICommand
        {
            public Guid TenantId { get; set; }
            public DateTime SigningDateTime { get; set; }
        }
    
}
