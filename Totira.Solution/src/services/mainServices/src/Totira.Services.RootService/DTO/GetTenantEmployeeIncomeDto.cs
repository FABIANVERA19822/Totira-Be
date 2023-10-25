namespace Totira.Services.RootService.DTO;

public class GetTenantEmployeeIncomeDto
{
    public Guid IncomeId { get; set; }
    public string CompanyOrganizationName { get; set; } = default!;
    public string Position { get; set; } = default!;
    public int? MonthlyIncome { get; set; }
    public string StartDate { get; set; } = default!;
    public string? EndDate { get; set; }
    public bool IsCurrentlyWorking { get; set; }
    public EmploymentIncomeContactReferenceDto ContactReference { get; set; } = default!;
    public IEnumerable<EmployeeIncomeFileDto>? Files { get; set; }
}

public class EmployeeIncomeFileDto
{
    public string FileName { get; set; } = default!;
}

public class EmploymentIncomeContactReferenceDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string JobTitle { get; set; } = default!;
    public string Relationship { get; set; } = default!;
    public string Email { get; set; } = default!;
    public EmploymentContactReferencePhoneNumberDto PhoneNumber { get; set; } = default!;

    public class EmploymentContactReferencePhoneNumberDto
    {
        public string Value { get; set; } = default!;
        public string CountryCode { get; set; } = default!;
    }
}
