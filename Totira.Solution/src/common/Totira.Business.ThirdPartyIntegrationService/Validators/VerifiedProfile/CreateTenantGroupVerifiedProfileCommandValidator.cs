
using Totira.Business.ThirdPartyIntegrationService.Commands.VerifiedProfile;
using Totira.Support.Application.Messages;

namespace Totira.Business.ThirdPartyIntegrationService.Validators.VerifiedProfile
{
    public class CreateTenantGroupVerifiedProfileCommandValidator : IMessageValidator<CreateTenantGroupVerifiedProfileCommand>
    {
        public ValidationResult Validate(CreateTenantGroupVerifiedProfileCommand command)
        {
            List<string> errors = new List<string>();

            return new ValidationResult(errors);
        }
    }
}
