using Totira.Bussiness.ThirdPartyIntegrationService.Commands.VerifiedProfile;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.ThirdPartyIntegrationService.Validators.VerifiedProfile
{
    public class CreateTenantVerifiedProfileCommandValidator : IMessageValidator<CreateTenantVerifiedProfileCommand>
    {
        public ValidationResult Validate(CreateTenantVerifiedProfileCommand command)
        {
            List<string> errors = new List<string>();
            
            return new ValidationResult(errors);
        }
    }
}
