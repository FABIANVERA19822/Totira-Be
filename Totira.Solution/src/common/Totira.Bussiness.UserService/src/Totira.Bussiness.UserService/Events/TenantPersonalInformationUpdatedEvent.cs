using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantPersonalInformationUpdatedEvent")]
    public class TenantPersonalInformatioUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public TenantPersonalInformatioUpdatedEvent(Guid id) => Id = id;
    }
}
