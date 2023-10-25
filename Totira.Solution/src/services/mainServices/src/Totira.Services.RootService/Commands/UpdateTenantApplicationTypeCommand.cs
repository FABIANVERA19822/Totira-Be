using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{ 
    [RoutingKey("UpdateTenantApplicationTypeCommand")]
    public class UpdateTenantApplicationTypeCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public string ApplicationType { get; set; } = string.Empty;
    }
}
