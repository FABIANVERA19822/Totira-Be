

namespace Totira.Services.RootService.Commands
{
    using Totira.Support.Application.Commands;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("DeleteTenantEmployeeIncomeIdCommand")]
    public class DeleteTenantEmployeeIncomeIdCommand: ICommand
    {
        public DeleteTenantEmployeeIncomeIdCommand(Guid tenantId, Guid incomeId)
        {
            TenantId = tenantId;
            IncomeId = incomeId;
        }

        public Guid TenantId { get; set; }
        public Guid IncomeId { get; set; }   
    }
}
 

