using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantPersonalInformationUpdatedEvent")]
    public class TenantPersonalInformationUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string Message { get; }


        public TenantPersonalInformationUpdatedEvent(Guid id, string message)
        {
            this.Id = id;
            this.Message = message;
        }
    }
}
