﻿using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.Common;
using Totira.Bussiness.UserService.DTO.Files;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Bussiness.UserService.DTO.GetTenantProfileImageDto;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Common
{
    public class CommonFunctions : ICommonFunctions
    {
        private readonly IS3Handler _s3Handler;
        private readonly IRepository<TenantProfileImage, Guid> _tenantProfileImageRepository;
        private readonly ILogger<CommonFunctions> _logger;


        public CommonFunctions(
            IS3Handler s3Handler,
            IRepository<TenantProfileImage, Guid> tenantProfileImageRepository,
            ILogger<CommonFunctions> logger)
        {
            _s3Handler = s3Handler;
            _tenantProfileImageRepository = tenantProfileImageRepository;
            _logger = logger;
        }

        public async Task<GetTenantProfileImageDto> GetProfilePhoto(QueryTenantProfileImageById query)
        {
            //obtaining info from repository
            var info = await _tenantProfileImageRepository.GetByIdAsync(query.Id);

            if (info == null)
                return new GetTenantProfileImageDto(query.Id, new ProfileImageFile("", "", null));          


            //obtaining data from AWS S3
            string tenantId = query.Id.ToString();

            //create the key 
            var s3BucketKey = $"{tenantId}/{info.FileName}";          
            var ImageUrl = _s3Handler.GetPreSignedUrl(s3BucketKey);


            ProfileImageFile file = info != null ? new ProfileImageFile(info.FileName, info.ContentType, ImageUrl) : new ProfileImageFile("", "", null);


            return new GetTenantProfileImageDto(info.Id, file);
        }

        public async Task<Domain.Common.File?> UploadFileAsync(string keyName, FileInfoDto file)
        {
            using var ms = new MemoryStream(file.Data);
            var s3KeyName = $"{keyName}/{file.FileName}";

            var fileUploaded = await _s3Handler.UploadSMemorySingleFileAsync(
                keyName: s3KeyName,
                contentType: file.ContentType,
                fileMemoryStream: ms);

            if (!fileUploaded)
            {
                _logger.LogError("File {fileName} upload failed.", file.FileName);
                return default!;
            }

            _logger.LogInformation("File {fileName} upload to S3.", file.FileName);

            var entity = Domain.Common.File.Create(
                fileName: file.FileName,
                s3KeyName: s3KeyName,
                extension: file.ContentType,
                size: file.Size);
                
            return entity;
        }

        public async Task<bool> DeleteFileAsync(Domain.Common.File file)
        {
            var deleted = await _s3Handler.DeleteObjectAsync(file.S3KeyName);
            if (!deleted)
                _logger.LogError("File {fileName} delete failed.", file.FileName);
            else
                _logger.LogInformation("File {fileName} deleted from S3.", file.FileName);

            return deleted;
        }

        public async Task<DownloadFileDto> DownloadFileAsync(Domain.Common.File file)
        {
            var content = await _s3Handler.DownloadSingleFileAsync(file.S3KeyName);
            return new(file.Extension, file.FileName, content);
        }
    }
}
