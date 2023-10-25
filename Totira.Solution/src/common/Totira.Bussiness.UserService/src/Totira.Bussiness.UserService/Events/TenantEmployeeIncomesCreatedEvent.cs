using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantEmployeeIncomesCreatedEvent")]
    public class TenantEmployeeIncomesCreatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantEmployeeIncomesCreatedEvent(Guid id)
        {
            Id = id;
        }
    }
}
