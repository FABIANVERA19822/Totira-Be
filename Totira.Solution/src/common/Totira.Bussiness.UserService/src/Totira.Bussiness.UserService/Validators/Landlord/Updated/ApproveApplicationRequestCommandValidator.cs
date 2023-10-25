using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators.Landlord;

public class ApproveApplicationRequestCommandValidator : IMessageValidator<ApproveApplicationRequestCommand>
{
    public ValidationResult Validate(ApproveApplicationRequestCommand command)
    {
        var errors = new List<string>();

        if (command.PropertyApplicationId == Guid.Empty)
            errors.Add("PropertyApplicationId cannot be empty.");

        return new ValidationResult(errors);
    }
}
