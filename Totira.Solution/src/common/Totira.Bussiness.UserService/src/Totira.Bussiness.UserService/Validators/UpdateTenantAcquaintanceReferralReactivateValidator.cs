using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class UpdateTenantAcquaintanceReferralReactivateValidator : IMessageValidator<UpdateTenantAcquaintanceReferralReactivateCommand>
    {
        public ValidationResult Validate(UpdateTenantAcquaintanceReferralReactivateCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
