using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Support.CommonLibrary.CommonlHandlers.Utils;
using Totira.Support.CommonLibrary.Configurations;
using Totira.Support.CommonLibrary.Interfaces;

namespace Totira.Support.CommonLibrary.CommonlHandlers
{
    public class S3Handler : IS3Handler
    {
        private readonly IOptions<S3Settings> _configuration;
        private readonly ILogger<S3Handler> _logger;
        private TransferUtility transferUtility;
        public S3Handler(IOptions<S3Settings> configuration, ILogger<S3Handler> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> DownloadSingleFileAsync(string keyName, string localPath)
        {
            bool result = false;
            using (var client = new AmazonS3Client(_configuration.Value.AwsS3AccessKey, _configuration.Value.AwsS3AccessSecretKey, RegionEndpoint.USEast1))
            {
                transferUtility = new TransferUtility(client);
                await transferUtility.DownloadAsync(new TransferUtilityDownloadRequest
                {
                    BucketName = _configuration.Value.AwsBucketName,
                    Key = keyName,
                    FilePath = $"{localPath}\\{keyName}",
                });
                result = File.Exists($"{localPath}\\{keyName}");
            }
            return result;
        }


        public async Task<byte[]> DownloadSingleFileAsync(string keyName)
        {
            byte[] result = null;
            using (var client = new AmazonS3Client(_configuration.Value.AwsS3AccessKey, _configuration.Value.AwsS3AccessSecretKey, RegionEndpoint.USEast1))
            {
                var objResponse = await client.GetObjectAsync(_configuration.Value.AwsBucketName, keyName);
                MemoryStream memoryStream = new MemoryStream();

                if (objResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (Stream responseStream = objResponse.ResponseStream)
                    {
                        responseStream.CopyTo(memoryStream);

                        result = memoryStream.ToArray();
                    }
                }
                else
                {
                    _logger.LogCritical($"The file with the keyName {keyName} dont exist or have error on s3");
                    throw new Exception($"The file with the keyName {keyName} dont exist or have error on s3");
                }
            }
            return result;
        }

        public async Task<bool> ListingObjectsAsync()
        {
            bool result = true;
            using (var client = new AmazonS3Client(_configuration.Value.AwsS3AccessKey, _configuration.Value.AwsS3AccessSecretKey, RegionEndpoint.USEast1))
            {
                var listObjectsV2Paginator = client.Paginators.ListObjectsV2(new ListObjectsV2Request
                {
                    BucketName = _configuration.Value.AwsBucketName,
                });
                await foreach (var response in listObjectsV2Paginator.Responses)
                {
                    Console.WriteLine($"HttpStatusCode: {response.HttpStatusCode}");
                    Console.WriteLine($"Number of Keys: {response.KeyCount}");
                    foreach (var entry in response.S3Objects)
                    {
                        Console.WriteLine($"Key = {entry.Key} Size = {entry.Size}");
                    }
                }
                result = true;
            }
            return result;
        }

        public async Task<bool> UploadSingleFileAsync(string fileName, string localPath)
        {
            bool result = false;
            if (File.Exists($"{localPath}\\{fileName}"))
            {
                using (var client = new AmazonS3Client(_configuration.Value.AwsS3AccessKey, _configuration.Value.AwsS3AccessSecretKey, RegionEndpoint.USEast1))
                {
                    transferUtility = new TransferUtility(client);
                    await transferUtility.UploadAsync(new TransferUtilityUploadRequest
                    {
                        BucketName = _configuration.Value.AwsBucketName,
                        Key = fileName,
                        FilePath = $"{localPath}\\{fileName}",
                    });
                    result = true;
                }
            }
            else
            {
                Console.WriteLine($"{fileName} does not exist in {localPath}");
            }
            return result;
        }

        public async Task<bool> UploadSMemorySingleFileAsync(string keyName, string contentType, MemoryStream fileMemoryStream)
        {
            bool result = false;
            using (var client = new AmazonS3Client(_configuration.Value.AwsS3AccessKey, _configuration.Value.AwsS3AccessSecretKey, RegionEndpoint.USEast1))
            using (var transferUtility = new TransferUtility(client))
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    BucketName = _configuration.Value.AwsBucketName,
                    Key = keyName,
                    InputStream = fileMemoryStream,
                    ContentType = contentType
                };

                await transferUtility.UploadAsync(uploadRequest);
                result = true;
            }
            return result;
        }

        public async Task<bool> WritingAnObjectAsync(string keyName, string contentBody)
        {
            bool result = false;
            var putRequest = new PutObjectRequest
            {
                BucketName = _configuration.Value.AwsBucketName,
                Key = keyName,
                ContentBody = contentBody,
            };
            using (var client = new AmazonS3Client(_configuration.Value.AwsS3AccessKey, _configuration.Value.AwsS3AccessSecretKey))
            {
                var putResponse = await client.PutObjectAsync(putRequest);
                result = putResponse.HttpStatusCode == System.Net.HttpStatusCode.OK ? true : false;
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<byte[]?> GetObjectAsync(string key)
        {
            using var client = new AmazonS3Client(
                _configuration.Value.AwsS3AccessKey,
                _configuration.Value.AwsS3AccessSecretKey,
                RegionEndpoint.USEast1);

            byte[]? result = null;

            try
            {
                var request = new GetObjectRequest()
                {
                    BucketName = _configuration.Value.AwsBucketName,
                    Key = key
                };

                using var response = await client.GetObjectAsync(request);
                using var responseStream = response.ResponseStream;
                using var ms = new MemoryStream();
                await responseStream.CopyToAsync(ms);
                result = ms.ToArray();
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error getting key: {key}. Message: {ex.Message}");
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteObjectAsync(string key)
        {
            using var client = new AmazonS3Client(
                _configuration.Value.AwsS3AccessKey,
                _configuration.Value.AwsS3AccessSecretKey,
                RegionEndpoint.USEast1);

            bool result = false;

            try
            {
                var request = new DeleteObjectRequest()
                {
                    BucketName = _configuration.Value.AwsBucketName,
                    Key = key
                };

                await client.DeleteObjectAsync(request);

                result = true;
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.ErrorCode == "NoSuchKey")
                {
                    result = true;
                    _logger.LogWarning("S3 key does not exist but it returns true anyway.");
                }
                else
                {
                    _logger.LogError("Delete key failed. Error code: {code}. Message: {message}", ex.ErrorCode, ex.Message);
                }
            }

            return result;
        }


        public string GetPreSignedUrl(string key)
        {
            using var client = new AmazonS3Client(
                _configuration.Value.AwsS3AccessKey,
                _configuration.Value.AwsS3AccessSecretKey,
                RegionEndpoint.USEast1);


            var request = new GetPreSignedUrlRequest
            {
                BucketName = _configuration.Value.AwsBucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddDays(1)
            };


            var signedUrl = client.GetPreSignedURL(request);

            return signedUrl;


        }
    }




}
