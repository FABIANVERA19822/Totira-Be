
using Totira.Bussiness.UserService.DTO;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey(nameof(UpdateGroupApplicationCommand))]
    public class UpdateGroupApplicationCommand : ICommand
    {
        public UpdateGroupApplicationCommand()
        {
            GroupApplicationProfiles = new List<TenantGroupApplicationProfileDto>();
        }
        public List<TenantGroupApplicationProfileDto> GroupApplicationProfiles { get; set; }
    }

}
