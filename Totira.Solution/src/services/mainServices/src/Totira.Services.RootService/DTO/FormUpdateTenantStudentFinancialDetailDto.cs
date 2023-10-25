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
    public List<string> DeletedEnrollmentProofs { get; set; } = new();
    [Description("Proof of enrollment file")]
    [AllowedFileExtensions(new[] { "pdf", "png", "jpeg", "jpg" })]
    public List<IFormFile> EnrollmentProofs { get; set; } = new();
    [Description("Study permits or visas to be deleted")]
    public List<string> DeleteStudyPermitsOrVisas { get; set; } = new();
    [Description("Study permits or visas")]
    [AllowedFileExtensions(new[] { "pdf", "png", "jpeg", "jpg" })]
    public List<IFormFile> StudyPermitsOrVisas { get; set; } = new();
    [Description("Deleted proofs of income")]
    public List<string> DeleteIncomeProofs { get; set; } = new();
    [Description("New proofs of income")]
    [AllowedFileExtensions(new[] { "pdf", "png", "jpeg", "jpg" })]
    public List<IFormFile> IncomeProofs { get; set; } = new();
}