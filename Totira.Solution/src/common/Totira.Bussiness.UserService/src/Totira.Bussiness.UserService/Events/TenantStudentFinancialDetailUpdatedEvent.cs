using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events
{
    [RoutingKey("TenantStudentFinancialDetailUpdatedEvent")]
    public class TenantStudentFinancialDetailUpdatedEvent: IEvent
    {
        public Guid Id { get; set; }
        public string Message { get; }
        public TenantStudentFinancialDetailUpdatedEvent(Guid id, string message)
        {
            this.Id = id;
            this.Message = message;
        }
    }
}
