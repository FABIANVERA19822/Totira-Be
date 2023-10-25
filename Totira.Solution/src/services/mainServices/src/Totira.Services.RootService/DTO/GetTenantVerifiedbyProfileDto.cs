using System.ComponentModel.DataAnnotations;

namespace Totira.Services.RootService.DTO
{
    public class GetTenantVerifiedbyProfileDto
    {
        public Guid Id { get; set; }
        public string IdentityValidation { get; set; } = default!;
        public IncomeValidation EmployeeIncome { get; set; } = new();

        public CreditScore CreditRating { get; set; } = new();
        public BackgroundCheck BackgroundCheck { get; set; } = new();

    }

    public class BackgroundCheck
    {
        public string Status { get; set; } = string.Empty;
        public string CriminalConvictionStatus { get; set; } = string.Empty;
        public string SexOffenderStatus { get; set; } = string.Empty;
        public string FraudStatus { get; set; } = string.Empty;
        public string CourtRecordsStatus { get; set; } = string.Empty;
    }

    public class CreditScore
    {
        public string Status { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
    }

    public class IncomeValidation
    {
        public string Status { get; set; } = string.Empty;
        public List<EmployeeIncome> Employee { get; set; } = new();
        public Student Student { get; set; } = new();

    }

    public class Student
    {
        public string InstitutionName { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
    }

    public class EmployeeIncome
    {
        public int MonthlySalary { get; set; }
        public string Role { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
    }
}
