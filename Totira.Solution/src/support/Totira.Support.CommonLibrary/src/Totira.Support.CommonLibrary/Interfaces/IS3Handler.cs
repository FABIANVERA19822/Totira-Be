namespace Totira.Support.CommonLibrary.Interfaces
{
    public interface IS3Handler
    {
        Task<bool> WritingAnObjectAsync(string keyName, string contentBody);
        Task<bool> UploadSingleFileAsync(string fileName, string localPath);
        Task<bool> UploadSMemorySingleFileAsync(string keyName, string contentType, MemoryStream fileMemoryStream);
        Task<bool> DownloadSingleFileAsync(string keyName, string localPath);
        Task<byte[]> DownloadSingleFileAsync(string keyName);
        Task<bool> ListingObjectsAsync();
        /// <summary>
        /// Gets an object from S3 in byte array.
        /// </summary>
        /// <param name="key">S3 object key name.</param>
        /// <returns>Object byte array or null</returns>
        Task<byte[]?> GetObjectAsync(string key);
        /// <summary>
        /// Deletes an object stored in AWS S3
        /// </summary>
        /// <param name="key">S3 object key name.</param>
        /// <returns>True if object was deleted.</returns>
        Task<bool> DeleteObjectAsync(string key);

        /// <summary>
        /// Return url of object in bucket
        /// </summary>
        /// <param name="key">s3 Object key</param>
        /// <returns>Url PreSigned</returns>
        string GetPreSignedUrl(string key);
    }
}
