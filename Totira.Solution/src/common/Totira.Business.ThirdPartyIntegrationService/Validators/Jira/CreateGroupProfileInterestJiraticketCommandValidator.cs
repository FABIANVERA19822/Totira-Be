using System;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Support.Application.Messages;

namespace Totira.Business.ThirdPartyIntegrationService.Validators.Jira
{
    public class CreateGroupProfileInterestJiraticketCommandValidator : IMessageValidator<CreateGroupProfileInterestJiraticketCommand>
    {
        public ValidationResult Validate(CreateGroupProfileInterestJiraticketCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}

