namespace Totira.Bussiness.UserService.Validators
{
    using Totira.Bussiness.UserService.Commands;
    using Totira.Support.Application.Messages;

    public class UpdateApplicationRequestInvitationCommandValidator : IMessageValidator<UpdateApplicationRequestInvitationCommand>
    {
        public ValidationResult Validate(UpdateApplicationRequestInvitationCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
