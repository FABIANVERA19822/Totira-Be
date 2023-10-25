namespace Totira.Bussiness.UserService.Validators
{
    using Totira.Bussiness.UserService.Commands;
    using Totira.Support.Application.Messages;

    public class CreateTenantProfileImageCommandValidator : IMessageValidator<CreateTenantProfileImageCommand>
    {
        public ValidationResult Validate(CreateTenantProfileImageCommand command)
        {
            List<string> errors = new List<string>();


            return new ValidationResult(errors);
        }
    }
}
