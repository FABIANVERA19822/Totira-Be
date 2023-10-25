using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands.LandlordCommands;

[RoutingKey(nameof(CancelStatusPropertyApplicationCommand))]
public class CancelStatusPropertyApplicationCommand : ICommand
{
    /// <summary>
    /// Property application identifier
    /// </summary>
    public Guid PropertyApplicationId { get; set; }
}
