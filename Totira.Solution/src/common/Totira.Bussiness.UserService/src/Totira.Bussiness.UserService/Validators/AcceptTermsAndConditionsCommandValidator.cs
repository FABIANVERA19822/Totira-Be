using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class AcceptTermsAndConditionsCommandValidator : IMessageValidator<AcceptTermsAndConditionsCommand>
    {
       

        public ValidationResult Validate(AcceptTermsAndConditionsCommand command)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
