using Totira.Services.RootService.DTO.Common;

namespace Totira.Services.RootService.DTO;

public class GetTenantEmployeeIncomesDto
{
    public Guid TenantId { get; set; }
    public bool IsStudent { get; set; }
    public List<StudyDetailDto> StudyDetails { get; set; } = default!;
    public List<CurrentEmploymentDto> CurrentEmployements { get; set; } = default!;
    public List<PastEmploymentDto> PastEmployments { get; set; } = default!;
}

public class EmployeeIncomeDto
{
    public Guid IncomeId { get; set; }
    public string CompanyOrganizationName { get; set; } = default!;
    public string Position { get; set; } = default!;
    public List<TenantFileDisplayDto> Files { get; set; } = default!;
}

public class CurrentEmploymentDto : EmployeeIncomeDto
{
    public int MonthlyIncome { get; set; }
    public int DocumentsCount { get; set; }
    public string StartDate { get; set; } = default!;
    public string EndDate { get; set; } = default!;
    public string ProofOfIncome { get; set; } = default!;
    public CurrentEmploymentContactReferenceDto ContactReference { get; set; } = default!;

    public class CurrentEmploymentContactReferenceDto
    {
        [Mask]
        public string FirstName { get; set; } = default!;
        [Mask]
        public string LastName { get; set; } = default!;
        public string JobTitle { get; set; } = default!;
        public string Relationship { get; set; } = default!;
        [Mask]
        public string Email { get; set; } = default!;
        [Mask]
        public string Phone { get; set; } = default!;
    }
}

public class PastEmploymentDto : EmployeeIncomeDto
{
    public string Period { get; set; } = default!;
}

public class StudyDetailDto
{
    public Guid StudyId { get; set; }
    public string UniversityOrInstitute { get; set; } = default!;
    public string Degree { get; set; } = default!;
    public bool IsOverseasStudent { get; set; }
    public List<TenantFileDisplayDto>? EnrollmentProofs { get; set; }
    public List<TenantFileDisplayDto>? StudyPermitsOrVisas { get; set; }
    public List<TenantFileDisplayDto>? IncomeProofs { get; set; }
}