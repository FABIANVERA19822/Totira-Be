using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events;

[RoutingKey(nameof(ApplicationRequestApprovedEvent))]
public class ApplicationRequestApprovedEvent : BaseValidatedEvent
{
    /// <summary>
    /// <see cref="TenantPropertyApplication"/> Id
    /// </summary>
    public Guid Id { get; set; }

    public ApplicationRequestApprovedEvent(Guid id) => Id = id;

    public ApplicationRequestApprovedEvent()
    {
    }
}
