using System.ComponentModel.DataAnnotations;

namespace Totira.Services.RootService.Attributes;

public class FileSizeInAllItemsAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;
    private readonly int _minFileSize;

    public FileSizeInAllItemsAttribute(int maxFileSize, int minFileSize)
    {
        _maxFileSize = maxFileSize;
        _minFileSize = minFileSize;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is List<IFormFile> files && files.Any(x => x.Length > _maxFileSize || x.Length < _minFileSize))
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}
