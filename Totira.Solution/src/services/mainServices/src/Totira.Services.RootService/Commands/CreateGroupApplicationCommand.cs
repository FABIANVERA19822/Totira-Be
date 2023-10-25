
using Totira.Services.RootService.DTO;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("CreateGroupApplicationCommand")]
    public class CreateGroupApplicationCommand : ICommand
    {
        public CreateGroupApplicationCommand()
        {
            GroupApplicationProfiles = new List<TenantGroupApplicationProfileDto>();
        }
        public List<TenantGroupApplicationProfileDto> GroupApplicationProfiles { get; set; }
    }
}
