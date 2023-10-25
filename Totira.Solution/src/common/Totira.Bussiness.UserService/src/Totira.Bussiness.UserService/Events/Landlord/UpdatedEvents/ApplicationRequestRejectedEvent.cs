using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events;

[RoutingKey(nameof(ApplicationRequestRejectedEvent))]
public class ApplicationRequestRejectedEvent : BaseValidatedEvent
{
    public Guid Id { get; set; }

    public ApplicationRequestRejectedEvent(Guid id) => Id = id;

    public ApplicationRequestRejectedEvent()
    {
    }
}
