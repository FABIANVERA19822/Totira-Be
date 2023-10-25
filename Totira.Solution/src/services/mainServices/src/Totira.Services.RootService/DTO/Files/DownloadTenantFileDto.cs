namespace Totira.Services.RootService.DTO.Files;

public record DownloadTenantFileDto(string ContentType, string FileName, byte[] Content);