using Totira.Services.RootService.Attributes;
using Totira.Services.RootService.Commands;

namespace Totira.Services.RootService.DTO
{
    public class FormCreateTenantEmployeeIncomesDto
    {
        public Guid TenantId { get; set; }
        public string CompanyOrganizationName { get; set; } = default!;
        public string Position { get; set; } = default!;
        public int? MonthlyIncome { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentlyWorking { get; set; }
        public EmploymentContactReference ContactReference { get; set; } = new();
        [AllowedFileExtensions(new[] { "pdf", "png", "jpeg", "jpg" })]
        public List<IFormFile> Files { get; set; } = new();
    }
}
