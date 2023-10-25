using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands.LandlordCommands;

[RoutingKey(nameof(CancelStatusPropertyApplicationCommand))]
public class CancelStatusPropertyApplicationCommand : ICommand
{
    /// <summary>
    /// Property application identifier
    /// </summary>
    [Required]
    public Guid PropertyApplicationId { get; set; }
}
