using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.DTO.Common;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.FileUpload;

public static class FileExtensions
{
    public static bool HaveDuplicates(this List<TenantFile> files, TenantFileInfoDto file, out int coincidences)
    {
        var originalFileName = file.FileName;
        int dotIndex = originalFileName.LastIndexOf(".");
        var nameWithoutExtension = originalFileName.Substring(0, dotIndex);
        var found = files.Where(x => x.FileName.StartsWith(nameWithoutExtension));
        coincidences = 0;

        if (found.Any())
            coincidences = found.Count();
        
        return coincidences > 0;
    }

    public static void RenameWhenIsDuplicated(this TenantFileInfoDto fileInfoDto, int count)
    {
        var originalFileName = fileInfoDto.FileName;
        int dotIndex = originalFileName.LastIndexOf(".");

        if (dotIndex > 0)
        {
            string nameWithoutExtension = originalFileName.Substring(0, dotIndex);
            string extension = originalFileName.Substring(dotIndex);

            string newFileName = nameWithoutExtension + " (" + count + ")" + extension;
            fileInfoDto.FileName = newFileName;
        }
    }
}