
namespace Totira.Bussiness.UserService.Events
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("TenantApplicationTypeCreatedEvent")]
    public class TenantApplicationTypeCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantApplicationTypeCreatedEvent(Guid id) => Id = id;
    }
}

