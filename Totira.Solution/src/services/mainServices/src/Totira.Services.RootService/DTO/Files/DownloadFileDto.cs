namespace Totira.Bussiness.UserService.DTO.Files;

public record DownloadFileDto(string ContentType, string FileName, byte[] Content);
