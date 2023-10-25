
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantEmployeeIncomesUpdatedEvent")]
    public class TenantEmployeeIncomesUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string Message { get; }     

        public TenantEmployeeIncomesUpdatedEvent(Guid id, string message) 
        {
            this.Id = id;
            this.Message = message;
        }
    }
}
