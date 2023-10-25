using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators.Landlord.Updated;

public class CancelStatusPropertyApplicationCommandValidator : IMessageValidator<CancelStatusPropertyApplicationCommand>
{
    public ValidationResult Validate(CancelStatusPropertyApplicationCommand message)
    {
        var errors = new List<string>();

        if (message.PropertyApplicationId == Guid.Empty)
            errors.Add("PropertyApplicationId cannot be empty.");

        return new ValidationResult(errors);
    }
}
