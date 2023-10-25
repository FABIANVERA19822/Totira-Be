using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands;

[RoutingKey(nameof(ApproveApplicationRequestCommand))]
public class ApproveApplicationRequestCommand : ICommand
{
    /// <summary>
    /// Property application identifier
    /// </summary>
    public Guid PropertyApplicationId { get; set; }
}
