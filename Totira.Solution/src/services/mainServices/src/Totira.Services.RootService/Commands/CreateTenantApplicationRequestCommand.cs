using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("CreateTenantApplicationRequestCommand")]
    public class CreateTenantApplicationRequestCommand : ICommand
    {        
        public Guid TenantId { get; set; }
        public bool? ToLatest { get; set; }
        public Guid ApplicationDetailsId { get; set; }

    }
}
