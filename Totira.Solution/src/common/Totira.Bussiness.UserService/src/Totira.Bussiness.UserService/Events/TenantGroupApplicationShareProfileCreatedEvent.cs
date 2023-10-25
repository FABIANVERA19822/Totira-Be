namespace Totira.Bussiness.UserService.Events
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("TenantGroupApplicationShareProfileCreatedEvent")]
    public class TenantGroupApplicationShareProfileCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public TenantGroupApplicationShareProfileCreatedEvent(Guid id) => Id = id;
    }

}
