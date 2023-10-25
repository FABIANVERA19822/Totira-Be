using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class DeleteTenantApplicationRequestGuarantorCommandValidator : IMessageValidator<DeleteTenantApplicationRequestGuarantorCommand>
    {
        ValidationResult IMessageValidator<DeleteTenantApplicationRequestGuarantorCommand>.Validate(DeleteTenantApplicationRequestGuarantorCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
