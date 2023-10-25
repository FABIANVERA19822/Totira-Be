using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using Totira.Business.ThirdPartyIntegrationService.Events.Jira;
using Totira.Business.ThirdPartyIntegrationService.Options;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Jira
{
    public class TenantUpdateJiraTicketEmployeeIncomeCommandHandler : IMessageHandler<UpdateTenantEmployeeAndIncomeTicketJiraCommand>
    {
        private readonly IRepository<TenantEmployeeInconmeValidation, string> _tenantJiraValidationRepository;
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfileRepository;
        private readonly JiraOptions _jiraOptions;
        private readonly ILogger<TenantCreateJiraTicketEmployeeIncomeCommandHandler> _logger;

        public TenantUpdateJiraTicketEmployeeIncomeCommandHandler(
            IRepository<TenantEmployeeInconmeValidation, string> tenantJiraValidationRepository,
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository,
            IOptions<JiraOptions> jiraOptions,
            ILogger<TenantCreateJiraTicketEmployeeIncomeCommandHandler> logger
            )
        {
            _tenantJiraValidationRepository = tenantJiraValidationRepository;
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;  
            _jiraOptions = jiraOptions.Value;
            _logger = logger;
        }

        public async Task HandleAsync(IContext context, UpdateTenantEmployeeAndIncomeTicketJiraCommand command)
        {

            _logger.LogInformation($"updating information from jira for issue {command.Issue.id}");
            var issueId = command.Issue.id;
            var stored = await _tenantJiraValidationRepository.GetByIdAsync(issueId);

            if (stored == null)
            {
                return;
            }

            stored.UpdatedOn = DateTime.UtcNow;
            stored.Status = command.Issue.fields.status.name;

            var hasComments = command.Comment != null;

            if (hasComments)
            {
                if (stored.Comments != null)
                {
                    stored.Comments.Add(command.Comment.body);
                }
                else
                {
                    if (command.Comment != null)
                    {
                        stored.Comments = new List<string>() { command.Comment.body };
                    }

                }
            }

            await _tenantJiraValidationRepository.Update(stored);


            if (stored.Status.ToLower() == _jiraOptions.StatusComplete.ToLower() || stored.Status.ToLower() == _jiraOptions.StatusRejected.ToLower())
            {
                Expression<Func<TenantVerifiedProfile, bool>> expression = (p => p.TenantId == stored.TenantId);
                var verifications = (await _tenantVerifiedProfileRepository.Get(expression)).FirstOrDefault();
                verifications.Jira = true;
                verifications.UpdatedOn = DateTime.UtcNow;
                await _tenantVerifiedProfileRepository.Update(verifications);

            }
            else
            {

            }

            var inquiryUpdatedEvent = new TenantJiraTicketEmployeeIncomeUpdatedEvent(stored.TenantId);
        }
    }
}
