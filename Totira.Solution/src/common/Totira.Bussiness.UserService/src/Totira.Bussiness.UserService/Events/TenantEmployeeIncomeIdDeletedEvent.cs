namespace Totira.Bussiness.UserService.Events
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("TenantEmployeeIncomeIdDeletedEvent")]
    public class TenantEmployeeIncomeIdDeletedEvent : IEvent
    {
       public Guid Id { get; set; }
       public Guid IncomeId { get; set; }
         
        public TenantEmployeeIncomeIdDeletedEvent(Guid id, Guid incomeId)
        {
            Id = id;
            IncomeId = incomeId;
        }
    }
}
 