using System.ComponentModel.DataAnnotations;

namespace Totira.Services.RootService.DTO.Files.Request;

public class GetTenantFileRequest
{
    [Required]
    public string FileName { get; set; } = default!;
    [Required]
    public Guid TenantId { get; set; }
    public Guid? IncomeId { get; set; }
}
