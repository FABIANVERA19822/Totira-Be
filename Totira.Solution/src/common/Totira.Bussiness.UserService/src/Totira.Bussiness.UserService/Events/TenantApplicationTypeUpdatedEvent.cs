
namespace Totira.Bussiness.UserService.Events
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("TenantApplicationTypeUpdatedEvent")]
    public class TenantApplicationTypeUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantApplicationTypeUpdatedEvent(Guid id) => Id = id;

    }
}
