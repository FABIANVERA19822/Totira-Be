using Totira.Bussiness.UserService.Domain.Common;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain;

public sealed class TenantStudentFinancialDetail : Document
{
    private TenantStudentFinancialDetail(
        string universityOrInstitute,
        string degree,
        bool isOverseasStudent)
    {
        Id = Guid.NewGuid();
        UniversityOrInstitute = universityOrInstitute;
        Degree = degree;
        IsOverseasStudent = isOverseasStudent;
    }

    public string UniversityOrInstitute { get; set; }
    public string Degree { get; set; }
    public bool IsOverseasStudent { get; set; }

    public List<Common.File> EnrollmentProofs { get; set; } = new();
    public List<Common.File> StudyPermitsOrVisas { get; set; } = new();
    public List<Common.File> IncomeProofs { get; set; } = new();

    public static TenantStudentFinancialDetail Create(
        string universityOrInstitute,
        string degree,
        bool isOverseasStudent)
        => new(
            universityOrInstitute,
            degree,
            isOverseasStudent);

    public void UpdateInformation(
        string universityOrInstitute,
        string degree,
        bool isOverseasStudent)
    {
        UniversityOrInstitute = universityOrInstitute;
        Degree = degree;
        IsOverseasStudent = isOverseasStudent;
    }
}
