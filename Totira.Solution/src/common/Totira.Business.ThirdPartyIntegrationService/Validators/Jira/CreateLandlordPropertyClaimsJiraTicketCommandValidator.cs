
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Support.Application.Messages;

namespace Totira.Business.ThirdPartyIntegrationService.Validators.Jira
{
    public class CreateLandlordPropertyClaimsJiraTicketCommandValidator : IMessageValidator<CreateLandlordPropertyClaimsJiraTicketCommand>
    {
        public ValidationResult Validate(CreateLandlordPropertyClaimsJiraTicketCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
