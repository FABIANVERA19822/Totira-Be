using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("CreatePropertyTestCommand")]
    public class CreatePropertyTestCommand : ICommand
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

    }
}
