namespace Totira.Bussiness.UserService.DTO.Files;

public record DownloadTenantFileDto(string ContentType, string FileName, byte[] Content);
