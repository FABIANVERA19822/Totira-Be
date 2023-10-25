using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{

    public class UpdateTenantApplicationDetailsCommandValidator : IMessageValidator<UpdateTenantApplicationDetailsCommand>
    {
        public ValidationResult Validate(UpdateTenantApplicationDetailsCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
