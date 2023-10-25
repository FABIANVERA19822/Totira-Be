using Totira.Bussiness.UserService.Domain.Common;

namespace Totira.Bussiness.UserService.DTO.Common;

public class TenantFileDisplayDto
{
    protected TenantFileDisplayDto(string fileName, string contentType, long size)
    {
        FileName = fileName;
        ContentType = contentType;
        Size = size;
    }

    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }

    public static TenantFileDisplayDto Create(string fileName, string contentType, long size)
        => new(fileName, contentType, size);

    public static TenantFileDisplayDto? AdaptFrom(Domain.Common.File? entity)
        => entity is null
        ? default
        : new(entity.FileName, entity.Extension, entity.Size);

    public static List<TenantFileDisplayDto> AdaptFrom(IEnumerable<Domain.Common.File> files)
    {
        var result = new List<TenantFileDisplayDto>();

        foreach (var file in files)
            result.Add(Create(file.FileName, file.Extension, file.Size));

        return result;
    }
}
