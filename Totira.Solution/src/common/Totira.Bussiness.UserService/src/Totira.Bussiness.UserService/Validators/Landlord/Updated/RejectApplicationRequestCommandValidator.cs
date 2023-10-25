using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators.Landlord;

public class RejectApplicationRequestCommandValidator : IMessageValidator<RejectApplicationRequestCommand>
{
    public ValidationResult Validate(RejectApplicationRequestCommand command)
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
