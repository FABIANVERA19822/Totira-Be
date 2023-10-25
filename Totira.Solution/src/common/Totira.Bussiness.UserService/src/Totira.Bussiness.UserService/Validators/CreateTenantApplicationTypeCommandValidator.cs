namespace Totira.Bussiness.UserService.Validators
{
    using Totira.Bussiness.UserService.Commands;
    using Totira.Support.Application.Messages;

    public class CreateTenantApplicationTypeCommandValidator : IMessageValidator<CreateTenantApplicationTypeCommand>
    {
        public ValidationResult Validate(CreateTenantApplicationTypeCommand message)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(message.ApplicationType))
                errors.Add("Application Type is required."); 
            return new ValidationResult(errors);
        }
    }
}
