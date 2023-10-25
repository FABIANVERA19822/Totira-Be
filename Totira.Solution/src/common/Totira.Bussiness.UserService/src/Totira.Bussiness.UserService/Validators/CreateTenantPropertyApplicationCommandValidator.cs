namespace Totira.Bussiness.UserService.Validators
{
    using Totira.Bussiness.UserService.Commands;
    using Totira.Support.Application.Messages;
    public class CreateTenantPropertyApplicationCommandValidator : IMessageValidator<CreatePropertyToApplyCommand>
    {
        public ValidationResult Validate(CreatePropertyToApplyCommand command)
        {
            List<string> errors = new List<string>();


            return new ValidationResult(errors);
        }
    }
}
