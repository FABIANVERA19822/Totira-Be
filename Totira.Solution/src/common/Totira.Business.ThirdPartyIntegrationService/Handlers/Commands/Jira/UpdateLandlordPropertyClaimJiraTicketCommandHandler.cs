
using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;
using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using Totira.Business.ThirdPartyIntegrationService.Options;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Events.Jira;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Jira
{
    public class UpdateLandlordPropertyClaimJiraTicketCommandHandler : IMessageHandler<UpdateLandlordPropertyClaimJiraTicketCommand>
    {
        private readonly IRepository<LandlordPropertyClaimValidation, string> _landlordPropertyValidationRepository;
        private readonly IRepository<TenantVerifiedProfile, Guid> _tenantVerifiedProfileRepository;
        private readonly JiraOptions _jiraOptions;
        private readonly ILogger<UpdateLandlordPropertyClaimJiraTicketCommandHandler> _logger;
        private readonly IEventBus _bus;
        private readonly IContextFactory _contextFactory;

        public UpdateLandlordPropertyClaimJiraTicketCommandHandler(
            IRepository<LandlordPropertyClaimValidation, string> landlordPropertyValidationRepository,
            IRepository<TenantVerifiedProfile, Guid> tenantVerifiedProfileRepository,
            IOptions<JiraOptions> jiraOptions,
            ILogger<UpdateLandlordPropertyClaimJiraTicketCommandHandler> logger,
            IEventBus bus,
            IContextFactory contextFactory
            )
        {
            _landlordPropertyValidationRepository = landlordPropertyValidationRepository;
            _tenantVerifiedProfileRepository = tenantVerifiedProfileRepository;
            _jiraOptions = jiraOptions.Value;
            _logger = logger;
            _bus = bus;
            _contextFactory = contextFactory;
        }

        public async Task HandleAsync(IContext context, Either<Exception, UpdateLandlordPropertyClaimJiraTicketCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogInformation("updating information from Landlord jira for issue {IssueId}", cmd.Issue.id);
                var issueId = cmd.Issue.id;
                var stored = await _landlordPropertyValidationRepository.GetByIdAsync(issueId);

                if (stored == null)
                {
                    return;
                }

                stored.UpdatedOn = DateTime.UtcNow;
                stored.Status = cmd.Issue.fields.status.name;

                var rejectedComment = cmd.Issue.fields.customfield_10053?.ToString() ?? string.Empty;

                stored.Comments = new List<string>() { rejectedComment };

                await _landlordPropertyValidationRepository.Update(stored);

                if (stored.Status.ToLower() == _jiraOptions.StatusComplete.ToLower())
                {
                    // update property

                    // send email Acceptance
                    
                }
                else if (stored.Status.ToLower() == _jiraOptions.StatusRejected.ToLower())
                {
                    // update property

                    // send email Rejected

                }

                var inquiryUpdatedEvent = new LandlordPropertyClaimJiraTicketUpdatedEvent(stored.LandlordId);
            }, ex => {
                var inquiryUpdatedEvent = new LandlordPropertyClaimJiraTicketUpdatedEvent(Guid.NewGuid());
                throw ex;
            });
        }
    }
}