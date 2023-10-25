
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class CreateTenantCurrentJobStatusCommandValidator : IMessageValidator<CreateTenantCurrentJobStatusCommand>
    {
        public ValidationResult Validate(CreateTenantCurrentJobStatusCommand message)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(message.TenantId.ToString()))
                errors.Add("Tenant Id is required.");
            return new ValidationResult(errors);
        }
    }
}
