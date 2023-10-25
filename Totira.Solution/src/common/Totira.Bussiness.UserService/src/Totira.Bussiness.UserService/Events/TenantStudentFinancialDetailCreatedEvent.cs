using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantStudentFinancialDetailCreatedEvent")]
    public class TenantStudentFinancialDetailCreatedEvent: IEvent
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public TenantStudentFinancialDetailCreatedEvent(Guid id, string message)
        {
            Id = id;
            Message = message;
        }
    }
}
