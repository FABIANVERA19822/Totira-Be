namespace Totira.Bussiness.UserService.DTO.Common;

public class TenantFileInfoDto
{
    public string FileName { get; set; } = default!;
    public long Length { get; set; }
    public string ContentType { get; set; } = default!;
    public byte[] Data { get; set; } = default!;
}
