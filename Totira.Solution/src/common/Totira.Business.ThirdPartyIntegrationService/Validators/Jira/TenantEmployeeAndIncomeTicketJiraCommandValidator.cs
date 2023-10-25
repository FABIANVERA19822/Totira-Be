using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Support.Application.Messages;

namespace Totira.Business.ThirdPartyIntegrationService.Validators.Jira
{
    public class TenantEmployeeAndIncomeTicketJiraCommandValidator : IMessageValidator<TenantEmployeeAndIncomeTicketJiraCommand>
    {
        public ValidationResult Validate(TenantEmployeeAndIncomeTicketJiraCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
