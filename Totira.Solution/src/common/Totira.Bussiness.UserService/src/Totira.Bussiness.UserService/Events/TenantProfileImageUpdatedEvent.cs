namespace Totira.Bussiness.UserService.Events
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("TenantProfileImageUpdatedEvent")]
    public class TenantProfileImageUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantProfileImageUpdatedEvent(Guid id) => Id = id;

    }
}
