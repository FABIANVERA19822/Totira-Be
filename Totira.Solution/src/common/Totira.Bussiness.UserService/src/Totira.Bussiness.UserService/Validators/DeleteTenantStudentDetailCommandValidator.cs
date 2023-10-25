using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators;

public class DeleteTenantStudentDetailCommandValidator : IMessageValidator<DeleteTenantStudentDetailCommand>
{
    public ValidationResult Validate(DeleteTenantStudentDetailCommand message)
    {
        var errors = new List<string>();

        return new ValidationResult(errors);
    }
}