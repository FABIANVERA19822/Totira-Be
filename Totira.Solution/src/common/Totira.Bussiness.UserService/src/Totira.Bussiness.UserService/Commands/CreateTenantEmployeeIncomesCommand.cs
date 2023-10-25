using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("CreateTenantEmployeeIncomesCommand")]
    public class CreateTenantEmployeeIncomesCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public string CompanyOrganizationName { get; set; } = default!;
        public string Position { get; set; } = default!;
        public int? MonthlyIncome { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentlyWorking { get; set; }
        public EmploymentContactReference ContactReference { get; set; } = default!;
        public List<EmployeeIncomeFile> Files { get; set; } = new();
    }

    public class EmployeeIncomeFile
    {
        public string FileName { get; set; } = default!;
        public int Size { get; set; }
        public string ContentType { get; set; } = default!;
        public byte[] Data { get; set; } = default!;

    }

    public class EmploymentContactReference
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string JobTitle { get; set; } = default!;
        public string Relationship { get; set; } = default!;
        public string Email { get; set; } = default!;
        public EmploymentContactReferencePhoneNumber PhoneNumber { get; set; } = default!;
    }

    public class EmploymentContactReferencePhoneNumber
    {
        public string Value { get; set; } = default!;
        public string CountryCode { get; set; } = default!;
    }
}
