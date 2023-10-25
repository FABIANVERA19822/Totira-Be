namespace Totira.Bussiness.UserService.Events
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("TenantApplicationRequestSendEmailsUpdatedEvent")]
    public class TenantApplicationRequestSendEmailsUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantApplicationRequestSendEmailsUpdatedEvent(Guid id) => Id = id;
    }
}
