using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class CreateTenantFeedbackViaLandlordCommandValidator : IMessageValidator<CreateTenantFeedbackViaLandlordCommand>
    {
        public ValidationResult Validate(CreateTenantFeedbackViaLandlordCommand command)
        {
            List<string> errors = new List<string>();

            if (command.Comment.Length > 500)
                errors.Add("Feedback exceed maximum limit (500)");

            return new ValidationResult(errors);
        }
    }
}
