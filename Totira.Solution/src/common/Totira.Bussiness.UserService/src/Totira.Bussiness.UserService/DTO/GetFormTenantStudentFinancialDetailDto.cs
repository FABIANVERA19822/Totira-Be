using Totira.Bussiness.UserService.DTO.Common;

namespace Totira.Bussiness.UserService.DTO;

public class GetFormTenantStudentFinancialDetailDto
{
    public GetFormTenantStudentFinancialDetailDto(
        string universityOrInstitute,
        string degree,
        bool isOverseasStudent,
        TenantFileDisplayDto proofOfEnrollment,
        TenantFileDisplayDto? studyPermitOrVisa,
        List<TenantFileDisplayDto> proofsOfIncome)
    {
        UniversityOrInstitute = universityOrInstitute;
        Degree = degree;
        IsOverseasStudent = isOverseasStudent;
        EnrollmentProof = proofOfEnrollment;
        StudyPermitOrVisa = studyPermitOrVisa;
        IncomeProofs = proofsOfIncome;
    }

    public string UniversityOrInstitute { get; set; }
    public string Degree { get; set; }
    public bool IsOverseasStudent { get; set; }
    public TenantFileDisplayDto EnrollmentProof { get; set; }
    public TenantFileDisplayDto? StudyPermitOrVisa { get; set; }
    public List<TenantFileDisplayDto> IncomeProofs { get; set; }
}
