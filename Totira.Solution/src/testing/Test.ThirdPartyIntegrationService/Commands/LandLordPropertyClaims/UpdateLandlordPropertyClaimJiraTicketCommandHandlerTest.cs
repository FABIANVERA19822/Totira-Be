using Microsoft.Extensions.Options;
using Moq;
using Test.ThirdPartyIntegrationService.Mocks.BusMock;
using Test.ThirdPartyIntegrationService.Mocks.RepoMocks.LandLordPropertyClaims;
using Test.ThirdPartyIntegrationService.Mocks.Settings;
using Totira.Business.ThirdPartyIntegrationService.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using Totira.Business.ThirdPartyIntegrationService.Handlers.Commands.Jira;
using Totira.Business.ThirdPartyIntegrationService.Options;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using static Totira.Support.Persistance.IRepository;

namespace Test.ThirdPartyIntegrationService.Commands.LandLordPropertyClaims
{
    public class UpdateLandlordPropertyClaimJiraTicketCommandHandlerTest : BaseCommandHandlerTest<UpdateLandlordPropertyClaimJiraTicketCommand, UpdateLandlordPropertyClaimJiraTicketCommandHandler>
    {
        private readonly Mock<IRepository<LandlordPropertyClaimValidation, string>> _landlordPropertyValidationRepository;
        private readonly Mock<IOptions<JiraOptions>> _jiraOptions;
        private readonly Mock<IEventBus> _bus;
        

        //command.Issue.fields.status.name
        //command.Issue.fields.customfield_10053
        public UpdateLandlordPropertyClaimJiraTicketCommandHandlerTest()
            : base(new UpdateLandlordPropertyClaimJiraTicketCommand
            {
                Issue = new Issue
                {
                    id = "1234567",
                    fields = new Fields
                    {
                        status = new Status { name = "Done" },
                        customfield_10053 = string.Empty
                    }
                },
            })
        {
            _landlordPropertyValidationRepository = MockLandlordPropertyClaimValidationRepository.GetMockLandlordPropertyClaimValidationRepository();
            _bus = MockIEventBus.GetIEventBusMock();
            _jiraOptions = MockIOptionsJiraOptions.GetIOptionsJiraMock();
        }

        protected override UpdateLandlordPropertyClaimJiraTicketCommandHandler CommandHandler => new UpdateLandlordPropertyClaimJiraTicketCommandHandler(
            _landlordPropertyValidationRepository.Object,
            _jiraOptions.Object,
            _loggerMock.Object,
            _bus.Object,
            _contextFactoryMock.Object,
            _messageServiceMock.Object);
        [Fact]
        public async Task HandlerAsyncTest_ApprovedJiraTicket()
        {
            
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command );
            var result = await _landlordPropertyValidationRepository.Object.GetByIdAsync(_command.Issue.id);
            Assert.NotNull(result);
            Assert.Equal(result.Status, "Done");
        }

        [Fact]
        public async Task HandlerAsyncTest_RejectedJiraTicket()
        {
            var command = new UpdateLandlordPropertyClaimJiraTicketCommand
            {
                Issue = new Issue
                {
                    id = "12345678",
                    fields = new Fields
                    {
                        status = new Status { name = "Rejected" },
                        customfield_10053 = string.Empty
                    }
                },
            };
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), command);
            var result = await _landlordPropertyValidationRepository.Object.GetByIdAsync(command.Issue.id);
            Assert.NotNull(result);
            Assert.Equal(result.Status, "Rejected");
        }
    }
}