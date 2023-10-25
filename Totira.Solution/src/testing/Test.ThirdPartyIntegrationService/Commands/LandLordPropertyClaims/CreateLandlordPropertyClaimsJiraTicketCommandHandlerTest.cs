using Microsoft.Extensions.Options;
using Moq;
using Test.ThirdPartyIntegrationService.Mocks.BusMock;
using Test.ThirdPartyIntegrationService.Mocks.RepoMocks.LandLordPropertyClaims;
using Test.ThirdPartyIntegrationService.Mocks.Settings;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Options;
using Totira.Support.EventServiceBus;
using static Totira.Support.Persistance.IRepository;

namespace Test.ThirdPartyIntegrationService.Commands.LandLordPropertyClaims
{
    internal class CreateLandlordPropertyClaimsJiraTicketCommandHandlerTest : BaseCommandHandlerTest<CreateLandlordPropertyClaimsJiraTicketCommand, CreateLandlordPropertyClaimsJiraTicketCommandHandler>
    {
        private readonly Mock<IRepository<LandlordPropertyClaimValidation, string>> _landlordPropertyValidationRepository;
        private readonly Mock<IOptions<JiraOptions>> _jiraOptions;
        private readonly Mock<IEventBus> _bus;

        public CreateLandlordPropertyClaimsJiraTicketCommandHandlerTest()
            : base(new CreateLandlordPropertyClaimsJiraTicketCommand
            {
                LandlordId = new Guid("1c05e52a-6cce-4216-8273-7bd6ef222ff7")
            })
        {
            _landlordPropertyValidationRepository = MockLandlordPropertyClaimValidationRepository.GetMockLandlordPropertyClaimValidationRepository();
            _bus = MockIEventBus.GetIEventBusMock();
            _jiraOptions = MockIOptionsJiraOptions.GetIOptionsJiraMock();
        }

        protected override CreateLandlordPropertyClaimsJiraTicketCommandHandler CommandHandler => throw new NotImplementedException();
    }
}