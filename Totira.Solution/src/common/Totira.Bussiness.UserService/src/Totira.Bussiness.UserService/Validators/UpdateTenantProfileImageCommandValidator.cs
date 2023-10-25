namespace Totira.Bussiness.UserService.Validators
{
    using Totira.Bussiness.UserService.Commands;
    using Totira.Support.Application.Messages;
    public class UpdateTenantProfileImageCommandValidator : IMessageValidator<UpdateTenantProfileImageCommand>
    {
        public ValidationResult Validate(UpdateTenantProfileImageCommand command)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
