using System;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Support.Application.Messages;

namespace Totira.Business.ThirdPartyIntegrationService.Validators.Jira
{
	public class CreateProfileInterestJiraTicketCommandValidator : IMessageValidator<CreateProfileInterestJiraTicketCommand>
    {

        public ValidationResult Validate(CreateProfileInterestJiraTicketCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}

