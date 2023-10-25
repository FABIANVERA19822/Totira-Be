using System.ComponentModel.DataAnnotations;
using System.Linq;

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
}
