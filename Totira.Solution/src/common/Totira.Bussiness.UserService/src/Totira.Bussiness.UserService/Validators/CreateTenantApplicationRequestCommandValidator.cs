using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class CreateTenantApplicationRequestCommandValidator : IMessageValidator<CreateTenantApplicationRequestCommand>
    {
        public ValidationResult Validate(CreateTenantApplicationRequestCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}


