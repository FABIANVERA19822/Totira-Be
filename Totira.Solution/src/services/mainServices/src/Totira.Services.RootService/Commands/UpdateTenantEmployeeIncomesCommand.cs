using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("UpdateTenantEmployeeIncomesCommand")]
    public class UpdateTenantEmployeeIncomesCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public Guid IncomeId { get; set; }
        public string CompanyOrganizationName { get; set; } = default!;
        public string Position { get; set; } = default!;
        public int? MonthlyIncome { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentlyWorking { get; set; }
        public EmploymentContactReference ContactReference { get; set; } = default!;
        public List<string> DeletedFiles { get; set; } = new();
        public List<EmployeeIncomeFile> NewFiles { get; set; } = new();
    }
}
