using Totira.Business.ThirdPartyIntegrationService.Commands.Persona;
using Totira.Support.Application.Messages;

namespace Totira.Business.ThirdPartyIntegrationService.Validators.Persona
{
    public class TenantPersonaValidationCommandValidator : IMessageValidator<TenantPersonaValidationCommand>
    {
        public ValidationResult Validate(TenantPersonaValidationCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
