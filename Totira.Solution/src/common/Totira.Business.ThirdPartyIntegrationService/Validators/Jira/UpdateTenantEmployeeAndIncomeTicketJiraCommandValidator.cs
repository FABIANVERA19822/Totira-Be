using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Support.Application.Messages;

namespace Totira.Business.ThirdPartyIntegrationService.Validators.Jira
{
    public class UpdateTenantEmployeeAndIncomeTicketJiraCommandValidator : IMessageValidator<UpdateTenantEmployeeAndIncomeTicketJiraCommand>
    {
        public ValidationResult Validate(UpdateTenantEmployeeAndIncomeTicketJiraCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
