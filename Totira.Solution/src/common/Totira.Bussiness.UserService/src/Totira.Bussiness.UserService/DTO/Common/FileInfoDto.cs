namespace Totira.Bussiness.UserService.DTO.Common;

public class FileInfoDto
{
    public string FileName { get; set; } = default!;
    public long Size { get; set; }
    public string ContentType { get; set; } = default!;
    public byte[] Data { get; set; } = default!;
}
