using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators.Landlord;

public class RejectApplicationRequestCommandValidator : IMessageValidator<RejectApplicationRequestCommand>
{
    public ValidationResult Validate(RejectApplicationRequestCommand command)
    {
        var errors = new List<string>();

        if (command.PropertyApplicationId == Guid.Empty)
            errors.Add("PropertyApplicationId cannot be empty.");

        return new ValidationResult(errors);
    }
}
