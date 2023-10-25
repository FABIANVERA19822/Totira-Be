using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
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
        public EmploymentContactReference ContactReference { get; set; } = new();
        public List<EmployeeIncomeFile> Files { get; set; } = new();
    }

    public class EmployeeIncomeFile
    {
        public string FileName { get; set; }
        public long Length { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }

        public EmployeeIncomeFile(string fileName, long length, string contentType, byte[] data)
        {
            FileName = fileName;
            Length = length;
            ContentType = contentType;
            Data = data;
        }
    }

    public class EmploymentContactReference
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? JobTitle { get; set; }
        public string? Relationship { get; set; }
        public string? Email { get; set; }
        public EmploymentContactReferencePhoneNumber? PhoneNumber { get; set; }
    }

    public class EmploymentContactReferencePhoneNumber
    {
        public string? Value { get; set; }
        public string? CountryCode { get; set; }
    }
}
