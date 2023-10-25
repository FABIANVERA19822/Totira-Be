namespace Totira.Services.RootService.DTO.Common;

public class TenantFileDisplayDto
{
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Size { get; set; }
}
