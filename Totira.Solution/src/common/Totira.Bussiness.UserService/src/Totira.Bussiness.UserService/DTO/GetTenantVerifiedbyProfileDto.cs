
namespace Totira.Bussiness.UserService.DTO
{
    public class GetTenantVerifiedbyProfileDto
    {
        public Guid Id { get; set; }
        public string IdentityValidation { get; set; }
        public IncomeValidation EmployeeIncome { get; set; } = new();

        public CreditScore CreditRating { get; set; } = new();
        public BackgroundCheck BackgroundCheck { get; set; } = new();


        public GetTenantVerifiedbyProfileDto(Guid id, string identityValidation, IncomeValidation employeeIncome, CreditScore creditRating, BackgroundCheck backgroundCheck)
        {
            Id = id;
            IdentityValidation = identityValidation;
            EmployeeIncome = employeeIncome;
            CreditRating = creditRating;
            BackgroundCheck = backgroundCheck;
        }

        public GetTenantVerifiedbyProfileDto(Guid id)
        { Id = id; }
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
        public List<string> Comments { get; set; } = new List<string>();
        public List<EmployeeIncome> Employee { get; set; } = new();
        public List<Student> Student { get; set; } = new();

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