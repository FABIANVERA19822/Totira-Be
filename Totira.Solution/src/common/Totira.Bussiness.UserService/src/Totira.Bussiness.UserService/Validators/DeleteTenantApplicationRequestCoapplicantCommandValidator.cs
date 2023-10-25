using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class DeleteTenantApplicationRequestCoapplicantCommandValidator : IMessageValidator<DeleteTenantApplicationRequestCoapplicantCommand>
    {
        public ValidationResult Validate(DeleteTenantApplicationRequestCoapplicantCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
