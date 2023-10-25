using System.ComponentModel.DataAnnotations;

namespace Totira.Services.RootService.Attributes;

public class FileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;
    private readonly int _minFileSize;

    public FileSizeAttribute(int maxFileSize, int minFileSize)
    {
        _maxFileSize = maxFileSize;
        _minFileSize = minFileSize;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file && (file.Length > _maxFileSize || file.Length < _minFileSize))
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}
