
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class UpdateTenantCurrentJobStatusCommandValidator : IMessageValidator<UpdateTenantCurrentJobStatusCommand>
    {
        public ValidationResult Validate(UpdateTenantCurrentJobStatusCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
