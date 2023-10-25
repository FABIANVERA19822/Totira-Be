using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators;

public class UpdateTenantShareProfileTermsValidator : IMessageValidator<UpdateTenantShareProfileTermsCommand>
{
    public ValidationResult Validate(UpdateTenantShareProfileTermsCommand message)
    {
        List<string> errors = new List<string>();
        return new ValidationResult(errors);
    }
}
