using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantStudentDetailDeletedEvent")]
    public class TenantStudentDetailDeletedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string Message { get; }

        public TenantStudentDetailDeletedEvent(Guid id, string message)
        {
            this.Id = id;
            this.Message = message;
        }
    }
}
