using Totira.Business.ThirdPartyIntegrationService.Commands.Certn;
using Totira.Support.Application.Messages;

namespace Totira.Business.ThirdPartyIntegrationService.Validators.Certn;

public class ApplyCertnGroupCommandValidator : IMessageValidator<ApplyCertnGroupCommand>
{
    public ValidationResult Validate(ApplyCertnGroupCommand command)
    {
        var errors = new List<string>();
        
        return new ValidationResult(errors);
    }
}