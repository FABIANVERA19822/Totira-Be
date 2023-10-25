namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Jira
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
    using Totira.Business.ThirdPartyIntegrationService.Commands.PropertyClaims;
    using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
    using Totira.Business.ThirdPartyIntegrationService.Events.Jira;
    using Totira.Business.ThirdPartyIntegrationService.Options;
    using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
    using Totira.Support.Application.Messages;
    using Totira.Support.EventServiceBus;
    using static Totira.Support.Persistance.IRepository;

    public class UpdateLandlordPropertyClaimJiraTicketCommandHandler : BaseMessageHandler<UpdateLandlordPropertyClaimJiraTicketCommand, LandlordPropertyClaimJiraTicketUpdatedEvent>
    {
        private readonly IRepository<LandlordPropertyClaimValidation, string> _landlordPropertyValidationRepository;
        private readonly JiraOptions _jiraOptions;
        private readonly IEventBus _bus;

        public UpdateLandlordPropertyClaimJiraTicketCommandHandler(
            IRepository<LandlordPropertyClaimValidation, string> landlordPropertyValidationRepository,
            IOptions<JiraOptions> jiraOptions,
            ILogger<UpdateLandlordPropertyClaimJiraTicketCommandHandler> logger,
            IEventBus bus,
            IContextFactory contextFactory,
            IMessageService messageService
            )
            : base(logger, contextFactory, messageService)
        {
            _landlordPropertyValidationRepository = landlordPropertyValidationRepository;
            _jiraOptions = jiraOptions.Value;
            _bus = bus;
        }

        protected override async Task<LandlordPropertyClaimJiraTicketUpdatedEvent> Process(IContext context, UpdateLandlordPropertyClaimJiraTicketCommand command)
        {
            _logger.LogInformation("updating information from Landlord jira for issue {IssueId}", command.Issue.id);
            var issueId = command.Issue.id;
            var stored = await _landlordPropertyValidationRepository.GetByIdAsync(issueId);

            if (stored == null)
            {
                return new LandlordPropertyClaimJiraTicketUpdatedEvent() { Errors = new List<string> { $"No record with id{command.Issue.id}" } };
            }

            stored.UpdatedOn = DateTime.UtcNow;
            stored.Status = command.Issue.fields.status.name;
            
            var rejectedComment = command.Issue.fields.customfield_10053?.ToString() ?? string.Empty;
            var mlsid = command.Issue.fields.customfield_10060?.ToString() ?? string.Empty;

            stored.Comments = new List<string>() { rejectedComment };

            await _landlordPropertyValidationRepository.Update(stored);

            if (stored.Status.ToLower() == _jiraOptions.StatusComplete.ToLower())
            {
                await SendUpdatePropertyClaims(stored, "Approved", string.Empty);
            }
            else if (stored.Status.ToLower() == _jiraOptions.StatusRejected.ToLower())
            {
                await SendUpdatePropertyClaims(stored, "Rejected", rejectedComment);
            }

            return new LandlordPropertyClaimJiraTicketUpdatedEvent(stored.LandlordId);
        }

        private async Task SendUpdatePropertyClaims(LandlordPropertyClaimValidation? stored, string status, string message)
        {
            var request = new UpdatePropertyClaimsFromJiraTicketCommand()
            {
                LandlordId = stored.LandlordId,
                ClaimId = stored.ClaimId,
                Status = status,
                Message = message,
                Address = stored.Address,
                MLSId = stored.Ml_num,
                Email = stored.Email
            };
            var userContext = _contextFactory.Create(string.Empty, Guid.Empty);

            await _bus.PublishAsync(userContext, request);
        }
    }
}