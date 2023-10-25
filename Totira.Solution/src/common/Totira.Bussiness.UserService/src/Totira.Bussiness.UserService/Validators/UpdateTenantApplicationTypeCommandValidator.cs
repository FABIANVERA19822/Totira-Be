
namespace Totira.Bussiness.UserService.Validators
{
    using Totira.Bussiness.UserService.Commands;
    using Totira.Support.Application.Messages;

    public class UpdateTenantApplicationTypeCommandValidator : IMessageValidator<UpdateTenantApplicationTypeCommand>
    {
        public ValidationResult Validate(UpdateTenantApplicationTypeCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
