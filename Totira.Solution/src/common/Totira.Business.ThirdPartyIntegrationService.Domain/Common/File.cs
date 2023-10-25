namespace Totira.Business.ThirdPartyIntegrationService.Domain.Common;

/// <summary>
/// A class that represents a tenant file info
/// </summary>
public class File
{
    /// <summary>
    /// Initializes a new instance of <see cref="File"/> class.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="s3KeyName"></param>
    /// <param name="extension"></param>
    /// <param name="size"></param>
    private File(string fileName, string s3KeyName, string extension, long size)
    {
        FileName = fileName;
        S3KeyName = s3KeyName;
        Extension = extension;
        Size = size;
    }

    public string FileName { get; set; }
    public string S3KeyName { get; set; }
    public string Extension { get; set; }
    public long Size { get; set; }

    /// <summary>
    /// Creates a new <see cref="File"/> object.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="s3KeyName">S3 Key name.</param>
    /// <param name="extension">File extension.</param>
    /// <param name="size">File size.</param>
    /// <returns></returns>
    public static File Create(string fileName, string s3KeyName, string extension, long size)
        => new(fileName, s3KeyName, extension, size);
}
