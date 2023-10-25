using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Totira.Services.RootService.Attributes;

namespace Totira.Services.RootService.DTO;

public class FormUpdateTenantStudentFinancialDetailDto
{
    [Required]
    [Description("Tenant Id")]
    public Guid TenantId { get; set; }
    [Required]
    [Description("Study Id")]
    public Guid StudyId { get; set; }
    [Required]
    [Description("University or institution tenant have enrolled with")]
    public string UniversityOrInstitute { get; set; } = default!;
    [Required]
    [Description("Degree tenant have enrolled in")]
    public string Degree { get; set; } = default!;
    [Required]
    [DefaultValue(false)]
    [Description("States if tenant is an overseas student")]
    public bool IsOverseasStudent { get; set; }
    [Description("Proof of enrollment files to be deleted")]
    public IList<string> DeletedEnrollmentProofs { get; set; } = default!;
    [Description("Proof of enrollment file")]
    [AllowedFileExtensions(new[] { "pdf", "png", "jpeg", "jpg" })]
    public IList<IFormFile> EnrollmentProofs { get; set; } = default!;
    [Description("Study permits or visas to be deleted")]
    public IList<string> DeleteStudyPermitsOrVisas { get; set; } = default!;
    [Description("Study permits or visas")]
    [AllowedFileExtensions(new[] { "pdf", "png", "jpeg", "jpg" })]
    public IList<IFormFile> StudyPermitsOrVisas { get; set; } = default!;
    [Description("Deleted proofs of income")]
    public IList<string> DeleteIncomeProofs { get; set; } = default!;
    [Description("New proofs of income")]
    [AllowedFileExtensions(new[] { "pdf", "png", "jpeg", "jpg" })]
    public IList<IFormFile> IncomeProofs { get; set; } = default!;
}