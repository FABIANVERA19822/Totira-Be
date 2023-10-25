


namespace Totira.Bussiness.UserService.Events
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;
    [RoutingKey("TenantApplicationTypeCreatedEvent")]
    public class TenantCurrentJobStatusCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantCurrentJobStatusCreatedEvent(Guid id) => Id = id;
    }
}
