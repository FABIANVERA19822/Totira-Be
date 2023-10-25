using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("CreatePropertyToApplyCommand")]
    public class CreatePropertyToApplyCommand : ICommand
    {
        public string PropertyId { get; set; }
        public Guid ApplicationRequestId { get; set; } 
        public Guid ApplicantId { get; set; } 
    }
}
