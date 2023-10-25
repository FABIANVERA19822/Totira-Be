using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantRentalHistoriesUpdatedEvent")]
    public class TenantRentalHistoriesUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public TenantRentalHistoriesUpdatedEvent(Guid id) => Id = id;


    }
}

