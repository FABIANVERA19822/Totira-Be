using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class CreateTenantApplicationDetailsCommandValidator : IMessageValidator<CreateTenantApplicationDetailsCommand>
    {
        public ValidationResult Validate(CreateTenantApplicationDetailsCommand command)
        {

            List<string> errors = new List<string>();
            if (command.Occupants.Adults < 1)
                errors.Add($"The occupants must be at less {1}");

            return new ValidationResult(errors);
        }
    }
}
