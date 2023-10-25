using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class CreateTenantBasicInformationCommandValidator : IMessageValidator<CreateTenantBasicInformationCommand>
    {
        public ValidationResult Validate(CreateTenantBasicInformationCommand command)
        {
            List<string> errors = new List<string>();

            //FirstName
            if (string.IsNullOrWhiteSpace(command.FirstName))
                errors.Add("FirstName is required.");

            //LastName
            if (string.IsNullOrWhiteSpace(command.LastName))
                errors.Add("LastName is required.");
          

            return new ValidationResult(errors);
        }
    }
}
