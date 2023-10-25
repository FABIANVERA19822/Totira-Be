
namespace Totira.Bussiness.UserService.Events
{

    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("TenantApplicationTypeCreatedEvent")]
    public class TenantApplicationTypeCreatedEvent
    {
        public Guid Id { get; set; }

        public TenantApplicationTypeCreatedEvent(Guid id) => Id = id;
    }
}

