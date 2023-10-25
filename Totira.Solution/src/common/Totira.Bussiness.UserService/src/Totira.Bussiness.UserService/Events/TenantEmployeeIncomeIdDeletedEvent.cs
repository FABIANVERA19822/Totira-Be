namespace Totira.Bussiness.UserService.Events
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("TenantEmployeeIncomeIdDeletedEvent")]
    public class TenantEmployeeIncomeIdDeletedEvent : IEvent
    {
       public Guid Id { get; set; }
       public Guid IncomeId { get; set; }
       public string Message { get; }
         
        public TenantEmployeeIncomeIdDeletedEvent(Guid id, Guid incomeId, string message)
        {
            this.Id = id;
            this.IncomeId = incomeId;
            this.Message = message;
        }
    }
}
 