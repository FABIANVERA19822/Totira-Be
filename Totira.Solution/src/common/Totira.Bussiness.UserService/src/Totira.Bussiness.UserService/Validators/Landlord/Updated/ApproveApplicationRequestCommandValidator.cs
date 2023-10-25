using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators.Landlord;

public class ApproveApplicationRequestCommandValidator : IMessageValidator<ApproveApplicationRequestCommand>
{
    public ValidationResult Validate(ApproveApplicationRequestCommand command)
    {
        var errors = new List<string>();

        if (command.TenantId == Guid.Empty)
            errors.Add("TenantId cannot be empty.");

        if (command.ApplicationRequestId == Guid.Empty)
            errors.Add("ApplicationRequestId cannot be empty.");

        if (string.IsNullOrWhiteSpace(command.PropertyId))
            errors.Add("PropertyId cannot be empty.");

        return new ValidationResult(errors);
    }
}
