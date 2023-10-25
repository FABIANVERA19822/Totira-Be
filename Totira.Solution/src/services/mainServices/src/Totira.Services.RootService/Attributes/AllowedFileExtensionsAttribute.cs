using System.ComponentModel.DataAnnotations;

namespace Totira.Services.RootService.Attributes;

public class AllowedFileExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _allowedExtensions;

    public AllowedFileExtensionsAttribute(string[] allowedExtensions)
    {
        _allowedExtensions = allowedExtensions;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var fileExtension = GetFileExtension(file.FileName);

            if (!IsValidExtension(fileExtension))
            {
                string allowedExtensions = string.Join(", ", _allowedExtensions);
                return new ValidationResult($"Invalid format file. Please upload a supported format file: {allowedExtensions}");
            }
        }

        if (value is List<IFormFile> files)
        {
            foreach (var item in files)
            {
                var fileExtension = GetFileExtension(item.FileName);

                if (!IsValidExtension(fileExtension))
                {
                    string allowedExtensions = string.Join(", ", _allowedExtensions);
                    return new ValidationResult($"Invalid format file. Please upload a supported format file: {allowedExtensions}");
                }
            }
        }

        return ValidationResult.Success;
    }

    private static string GetFileExtension(string fileName)
        => fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();

    private bool IsValidExtension(string fileExtension)
        => _allowedExtensions
            .Any(x => fileExtension.Equals(x, StringComparison.OrdinalIgnoreCase));

    public static IFormFile BuildImageFromBase64(string base64Url, string nameImage)
    {
        IFormFile file = null;
        byte[] bytes = Convert.FromBase64String(base64Url);
        
        MemoryStream stream = new MemoryStream(bytes);

        using (var memoryStream = new MemoryStream(bytes))
        {
            string contentType = validateImageFormat(memoryStream);

            file = new FormFile(stream, 0, bytes.Length, nameImage, nameImage)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        return file;
    }

    public static string validateImageFormat(MemoryStream memoryStream)
    {
        var contentType = GetMimeType(memoryStream, MimeTypes.Images);

        if (!contentType.StartsWith("image/"))
        {
            throw new Exception("The format is not the one allowed");
        }

        return contentType;
    }

    public static string validatePDFDocumentFormat(MemoryStream memoryStream)
    {
        var contentType = GetMimeType(memoryStream, MimeTypes.PDF);

        if (!contentType.StartsWith("application/pdf"))
        {
            throw new Exception("The format is not the one allowed");
        }

        return contentType;
    }

    public static string validateImagesAndPDFDocumentFormat(MemoryStream memoryStream)
    {
        var contentType = GetMimeType(memoryStream, MimeTypes.ImagesAndPDF);

        if (!contentType.StartsWith("image/") || !contentType.StartsWith("application/pdf"))
        {
            throw new Exception("The format is not the one allowed");
        }

        return contentType;
    }

    private static readonly Dictionary<string, string> MappingsImages = new Dictionary<string, string>
    {
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".png", "image/png" },
        // Add more mappings as needed
    };

    private static readonly Dictionary<string, string> MappingsPDF = new Dictionary<string, string>
    {
        { ".pdf", "application/pdf" },
        // Add more mappings as needed
    };

    public enum MimeTypes
    {
        Images = 0,
        PDF = 1,
        ImagesAndPDF = 3
    }

    public static string GetMimeType(Stream stream, MimeTypes mimeTypes)
    {
        var ext = GetFileExtension(stream);

        if (mimeTypes == MimeTypes.Images && MappingsImages.ContainsKey(ext))
        {
            return MappingsImages[ext];            
        }
        else if (mimeTypes == MimeTypes.PDF && MappingsPDF.ContainsKey(ext))
        {
            return MappingsPDF[ext];
        }
        else if (mimeTypes == MimeTypes.ImagesAndPDF && (MappingsPDF.ContainsKey(ext) || MappingsPDF.ContainsKey(ext)))
        {
            var mappings = new Dictionary<string, string>();
            mappings.Union(MappingsImages);
            mappings.Union(MappingsPDF);
            return mappings[ext];
        }

        return "application/octet-stream";
    }

    private static string GetFileExtension(Stream stream)
    {
        var ext = string.Empty;
        var buffer = new byte[8];
        stream.Read(buffer, 0, 8);

        if (buffer[0] == 'P' && buffer[1] == 'N' && buffer[2] == 'G')
        {
            ext = ".png";
        }
        else if (buffer[0] == 0xFF && buffer[1] == 0xD8)
        {
            ext = ".jpg";
        }
        else if (buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47)
        {
            ext = ".png";
        }
        else if (buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46)
        {
            ext = ".pdf";
        }
        // Add more checks for other image formats

        return ext;
    }
}
