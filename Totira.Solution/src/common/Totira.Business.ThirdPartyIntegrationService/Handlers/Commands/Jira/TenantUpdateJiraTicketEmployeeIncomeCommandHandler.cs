using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Business.ThirdPartyIntegrationService.Commands.CurrentJobStatus;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using Totira.Business.ThirdPartyIntegrationService.Events.Jira;
using Totira.Business.ThirdPartyIntegrationService.Options;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
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
        private readonly IEventBus _bus;
        private readonly IContextFactory _contextFactory;

        public TenantUpdateJiraTicketEmployeeIncomeCommandHandler(
            IRepository<TenantEmployeeInconmeValidation, string> tenantJiraValidationRepository,
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository,
            IOptions<JiraOptions> jiraOptions,
            ILogger<TenantCreateJiraTicketEmployeeIncomeCommandHandler> logger,
            IEventBus bus,
            IContextFactory contextFactory
            )
        {
            _tenantJiraValidationRepository = tenantJiraValidationRepository;
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;
            _jiraOptions = jiraOptions.Value;
            _logger = logger;
            _bus = bus;
            _contextFactory = contextFactory;
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

            var rejectedComment = command.Issue.fields.customfield_10053?.ToString() ?? string.Empty;

            stored.Comments = new List<string>() { rejectedComment };

            await _tenantJiraValidationRepository.Update(stored);

            if (stored.Status.ToLower() == _jiraOptions.StatusComplete.ToLower() || stored.Status.ToLower() == _jiraOptions.StatusRejected.ToLower())
            {
                Expression<Func<TenantVerifiedProfile, bool>> expression = (p => p.TenantId == stored.TenantId);
                var verifications = (await _tenantVerifiedProfileRepository.Get(expression)).FirstOrDefault();
                verifications.Jira = true;
                verifications.UpdatedOn = DateTime.UtcNow;
                await _tenantVerifiedProfileRepository.Update(verifications);
                var currentJob = new UpdateTenantCurrentJobStatusCommand()
                {
                    CurrentJobStatus = "",
                    TenantId = stored.TenantId,
                    IsUnderRevisionSend = false
                };

                var userContext = _contextFactory.Create(string.Empty, Guid.Empty);

                await _bus.PublishAsync(userContext, currentJob);
            }
            else
            {
            }

            var inquiryUpdatedEvent = new TenantJiraTicketEmployeeIncomeUpdatedEvent(stored.TenantId);
        }
    }
}