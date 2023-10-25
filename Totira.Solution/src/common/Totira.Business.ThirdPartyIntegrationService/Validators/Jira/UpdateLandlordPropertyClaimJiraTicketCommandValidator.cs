
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Support.Application.Messages;

namespace Totira.Business.ThirdPartyIntegrationService.Validators.Jira
{
    public class UpdateLandlordPropertyClaimJiraTicketCommandValidator : IMessageValidator<UpdateLandlordPropertyClaimJiraTicketCommand>
    {
        public ValidationResult Validate(UpdateLandlordPropertyClaimJiraTicketCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
