namespace Totira.Bussiness.UserService.Events
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("TenantProfileImageCreatedEvent")]
    public class TenantProfileImageCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantProfileImageCreatedEvent(Guid id) => Id = id;

    }
}
