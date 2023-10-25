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
        Task<TenantFile?> UploadFileAsync(string keyName, TenantFileInfoDto fileInfo);
        Task<bool> DeleteFileAsync(TenantFile file);
        Task<DownloadTenantFileDto> DownloadFileAsync(TenantFile file);
    }
}
