using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.Common;
using Totira.Bussiness.UserService.DTO.Files;
using Totira.Bussiness.UserService.Queries;

namespace Totira.Bussiness.UserService.Common
{
    public interface ICommonFunctions
    {
        Task<GetTenantProfileImageDto> GetProfilePhoto(QueryTenantProfileImageById query);
        Task<Domain.Common.File?> UploadFileAsync(string keyName, FileInfoDto fileInfo);
        Task<bool> DeleteFileAsync(Domain.Common.File file);
        Task<DownloadFileDto> DownloadFileAsync(Domain.Common.File file);
    }
}
