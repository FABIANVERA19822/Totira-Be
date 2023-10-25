using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Totira.Services.RootService.Attributes;

namespace Totira.Services.RootService.DTO;

public class FormCreateTenantStudentFinancialDetailDto
{
    [Required(ErrorMessage = "Tenant id is required.")]
    [Description("Tenant Id")]
    public Guid TenantId { get; set; }

    [Required(ErrorMessage = "University or Institution is required.")]
    [Description("University or institution tenant have enrolled with")]
    public string UniversityOrInstitute { get; set; } = default!;

    [Required(ErrorMessage = "Degree is required.")]
    [Description("Degree tenant have enrolled in")]
    public string Degree { get; set; } = default!;

    [Required]
    [DefaultValue(false)]
    [Description("States if tenant is an overseas student")]
    public bool IsOverseasStudent { get; set; }

    [Required]
    [Description("Proof of enrollment file")]
    [AllowedFileExtensions(new[]{ "pdf","png","jpeg", "jpg" })]
    [FileSize(20 * 1024 * 1024, 2048, ErrorMessage = "The file exceeds the supported size. Please try again with a smaller file.")]
    public List<IFormFile> EnrollmentProofs { get; set; } = default!;

    [Description("Study permit or visa")]
    [AllowedFileExtensions(new[] { "pdf", "png", "jpeg", "jpg" })]
    [FileSize(20 * 1024 * 1024, 2048, ErrorMessage = "The file exceeds the supported size. Please try again with a smaller file.")]
    public List<IFormFile>? StudyPermitsOrVisas { get; set; }

    [Required]
    [Description("Proofs of income")]
    [FileListCapacity(8)]
    [AllowedFileExtensions(new[] { "pdf", "png", "jpeg", "jpg" })]
    [FileSizeInAllItems(20 * 1024 * 1024, 2048, ErrorMessage = "The file exceeds the supported size. Please try again with a smaller file.")]
    public List<IFormFile> IncomeProofs { get; set; } = default!;
}
