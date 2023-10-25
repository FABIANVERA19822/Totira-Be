using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators;

public class DeleteTenantEmployeeIncomeFileValidator : IMessageValidator<DeleteTenantEmployeeIncomeFileCommand>
{
    public ValidationResult Validate(DeleteTenantEmployeeIncomeFileCommand message)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(message.FileName))
            errors.Add("File name cannot be empty.");

        return new ValidationResult(errors);
    }
}
