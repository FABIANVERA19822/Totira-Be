using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO.Common;

namespace Totira.Bussiness.UserService.DTO;

public class GetTenantEmployeeIncomesDto
{
    public GetTenantEmployeeIncomesDto(Guid tenantId)
    {
        TenantId = tenantId;
        CurrentEmployements = default!;
        StudyDetails = new();
        PastEmployments = new();
        CurrentEmployements = new();
    }

    public Guid TenantId { get; set; }
    public bool IsStudent { get; set; }
    public List<StudyDetailDto> StudyDetails { get; set; }
    public List<CurrentEmploymentDto> CurrentEmployements { get; set; }
    public List<PastEmploymentDto> PastEmployments { get; set; }
}

public abstract class EmployeeIncomeDto
{
    protected EmployeeIncomeDto(Guid incomeId,
        string companyOrganizationName,
        string position,
        List<TenantFileDisplayDto>? files)
    {
        IncomeId = incomeId;
        CompanyOrganizationName = companyOrganizationName;
        Position = position;
        Files = files ?? new();
    }

    public Guid IncomeId { get; set; }
    public string CompanyOrganizationName { get; set; }
    public string Position { get; set; }
    public List<TenantFileDisplayDto> Files { get; set; }
}

public class CurrentEmploymentDto : EmployeeIncomeDto
{
    protected CurrentEmploymentDto(Guid incomeId,
        string companyOrganizationName,
        string position,
        int documentsCount,
        string startDate,
        string proofOfIncome,
        int monthlyIncome,
        List<TenantFileDisplayDto>? files,
        CurrentEmploymentContactReferenceDto contactReference) :
        base(incomeId,
            companyOrganizationName,
            position,
            files)
    {
        MonthlyIncome = monthlyIncome;
        DocumentsCount = documentsCount;
        StartDate = startDate;
        ProofOfIncome = proofOfIncome;
        ContactReference = contactReference;
    }

    public int MonthlyIncome { get; set; }
    public int DocumentsCount { get; set; }
    public string StartDate { get; set; }
    public string ProofOfIncome { get; set; }
    public CurrentEmploymentContactReferenceDto ContactReference { get; set; }

    /// <summary>
    /// Adapts a <see cref="TenantEmployeeIncome"/> object to <see cref="CurrentEmploymentDto" />
    /// </summary>
    /// <param name="entity">Entity.</param>
    /// <returns>A new <see cref="CurrentEmploymentDto"/> object.</returns>
    public static CurrentEmploymentDto AdaptFrom(TenantEmployeeIncome entity)
        => new(entity.Id,
            entity.CompanyOrganizationName,
            entity.Position,
            entity.Files.Count,
            GetCurrentEmploymentFormatDate(entity.StartDate, GetMonthsOfDifference(entity.StartDate)),
            entity.Status,
            entity.MonthlyIncome!.Value,
            entity.Files.Select(file => TenantFileDisplayDto.Create(file.FileName, file.Extension, file.Size)).ToList(),
            CurrentEmploymentContactReferenceDto.AdaptFrom(entity.ContactReference));

    private static string GetCurrentEmploymentFormatDate(DateTime date, int monthsOfDifference)
    {
        if (monthsOfDifference <= 0)
            return $"{date:MMM d, yyyy}";
        if (monthsOfDifference == 1)
            return $"{date:MMM d, yyyy} (1 Month)";
        else
            return $"{date:MMM d, yyyy} ({monthsOfDifference} Months)";
    }

    private static int GetMonthsOfDifference(DateTime date) => (DateTime.Today - date).Days / 30;
}

public class CurrentEmploymentContactReferenceDto
{
    protected CurrentEmploymentContactReferenceDto(string firstName,
        string lastName,
        string jobTitle,
        string relationship,
        string email,
        string phoneNumberValue,
        string phoneNumberCountryCode)
    {
        FirstName = firstName;
        LastName = lastName;
        JobTitle = jobTitle;
        Relationship = relationship;
        Email = email;
        Phone = phoneNumberCountryCode + phoneNumberValue;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string JobTitle { get; set; }
    public string Relationship { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public static CurrentEmploymentContactReferenceDto AdaptFrom(EmploymentContactReference contactReference)
        => new(contactReference.FirstName,
            contactReference.LastName,
            contactReference.JobTitle,
            contactReference.Relationship,
            contactReference.Email,
            contactReference.PhoneNumber.Value,
            contactReference.PhoneNumber.CountryCode);
}

public class PastEmploymentDto : EmployeeIncomeDto
{
    protected PastEmploymentDto(Guid incomeId,
        string companyOrganizationName,
        string position,
        DateTime startDate,
        DateTime endDate,
        List<TenantFileDisplayDto>? files) :
        base(incomeId,
            companyOrganizationName,
            position,
            files)
    {
        Period = $"{startDate:MMM d, yyyy} - {endDate:MMM d, yyyy}";
    }
    public string Period { get; set; }

    public static PastEmploymentDto AdaptFrom(TenantEmployeeIncome entity)
        => new(entity.Id,
            entity.CompanyOrganizationName,
            entity.Position,
            entity.StartDate,
            entity.EndDate!.Value,
            entity.Files.Select(file => TenantFileDisplayDto.Create(file.FileName, file.Extension, file.Size)).ToList());
}

public class StudyDetailDto
{
    private StudyDetailDto(
        Guid studyId,
        string universityOrInstitute,
        string degree,
        bool isOverseasStudent,
        List<TenantFileDisplayDto>? enrollmentProofs,
        List<TenantFileDisplayDto>? studyPermitOrVisa,
        List<TenantFileDisplayDto>? incomeProofs)
    {
        StudyId = studyId;
        UniversityOrInstitute = universityOrInstitute;
        Degree = degree;
        IsOverseasStudent = isOverseasStudent;
        EnrollmentProofs = enrollmentProofs;
        StudyPermitsOrVisas = studyPermitOrVisa;
        IncomeProofs = incomeProofs;
    }

    public Guid StudyId { get; set; }
    public string UniversityOrInstitute { get; set; }
    public string Degree { get; set; }
    public bool IsOverseasStudent { get; set; }
    public List<TenantFileDisplayDto>? EnrollmentProofs { get; set; }
    public List<TenantFileDisplayDto>? StudyPermitsOrVisas { get; set; }
    public List<TenantFileDisplayDto>? IncomeProofs { get; set; }

    public static StudyDetailDto AdaptFrom(TenantStudentFinancialDetail entity)
        => new(
            entity.Id,
            entity.UniversityOrInstitute,
            entity.Degree,
            entity.IsOverseasStudent,
            TenantFileDisplayDto.AdaptFrom(entity.EnrollmentProofs),
            TenantFileDisplayDto.AdaptFrom(entity.StudyPermitsOrVisas),
            TenantFileDisplayDto.AdaptFrom(entity.IncomeProofs));
}