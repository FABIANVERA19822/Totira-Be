using System.ComponentModel.DataAnnotations;

namespace Totira.Services.RootService.Attributes;

public class FileListCapacityAttribute : ValidationAttribute
{
    private readonly int _capacity;
    private readonly int _minCapacity;

    public FileListCapacityAttribute(int capacity, int minCapacity = 0)
    {
        _capacity = capacity;
        _minCapacity = minCapacity;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is List<IFormFile> list && (list.Count > _capacity || list.Count < _minCapacity))
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}
