using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class CreateAcquaintanceReferralFormInfoCommandValidator : IMessageValidator<CreateAcquaintanceReferralFormInfoCommand>
    {
        public ValidationResult Validate(CreateAcquaintanceReferralFormInfoCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
