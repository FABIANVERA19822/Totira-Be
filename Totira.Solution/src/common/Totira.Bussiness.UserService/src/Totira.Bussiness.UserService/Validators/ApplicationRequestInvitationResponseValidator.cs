using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class ApplicationRequestInvitationResponseValidator : IMessageValidator<ApplicationRequestInvitationResponseCommand>
    {
        public ValidationResult Validate(ApplicationRequestInvitationResponseCommand command)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
