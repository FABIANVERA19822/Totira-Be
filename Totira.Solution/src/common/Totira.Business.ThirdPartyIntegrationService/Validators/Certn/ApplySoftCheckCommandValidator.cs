using Totira.Business.ThirdPartyIntegrationService.Commands.Certn;
using Totira.Support.Application.Messages;

namespace Totira.Business.ThirdPartyIntegrationService.Validators.Certn
{
    public class ApplySoftCheckCommandValidator : IMessageValidator<ApplySoftCheckCommand>
    {
        public ValidationResult Validate(ApplySoftCheckCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
