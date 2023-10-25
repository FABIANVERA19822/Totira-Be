using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq.Expressions;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantFeedbackViaLandlordCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantFeedbackViaLandlord, Guid>> _tenantFeedbackViaLandlordRepositoryMock;
        private readonly Mock<IRepository<TenantRentalHistories, Guid>> _tenantRentalHistoriesRepositoryMock;
        private readonly Mock<ILogger<CreateTenantFeedbackViaLandlordCommandHandler>> _loggerMock;
        private readonly Mock<IContextFactory> _contextFactoryMock;
        private readonly Mock<IMessageService> _messageServiceMock;
        private CreateTenantFeedbackViaLandlordCommand _command;

        public CreateTenantFeedbackViaLandlordCommandHandlerTest()
        {
            _tenantFeedbackViaLandlordRepositoryMock = MockTenantFeedbackViaLandlordRepository.GetTenantFeedbackViaLandlordRepository();
            _tenantRentalHistoriesRepositoryMock = MockTenantRentalHistoriesRepository.GetTenantRentalHistoriesRepository();
            _loggerMock = new Mock<ILogger<CreateTenantFeedbackViaLandlordCommandHandler>>();
            _contextFactoryMock = new Mock<IContextFactory>(MockBehavior.Strict);
            _messageServiceMock = new Mock<IMessageService>(MockBehavior.Strict);

            _command = new CreateTenantFeedbackViaLandlordCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                LandlordId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                StarScore = 5,
                Comment = "He is perfect man"
            };
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new CreateTenantFeedbackViaLandlordCommandHandler(_tenantFeedbackViaLandlordRepositoryMock.Object,
                                                                            _tenantRentalHistoriesRepositoryMock.Object,
                                                                            _loggerMock.Object,
                                                                            _contextFactoryMock.Object,
                                                                            _messageServiceMock.Object);
            Expression<Func<TenantFeedbackViaLandlord, bool>> feedbackExpression = (feed => feed.LandlordId == _command.LandlordId);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var landlordFeedback = (await _tenantFeedbackViaLandlordRepositoryMock.Object.Get(feedbackExpression)).FirstOrDefault();
            landlordFeedback.Score.ShouldBe(_command.StarScore);
        }

        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            //Arrange
            var handler = new CreateTenantFeedbackViaLandlordCommandHandler(_tenantFeedbackViaLandlordRepositoryMock.Object,
                                                                            _tenantRentalHistoriesRepositoryMock.Object,
                                                                            _loggerMock.Object,
                                                                            _contextFactoryMock.Object,
                                                                            _messageServiceMock.Object);
            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            Expression<Func<TenantFeedbackViaLandlord, bool>> filter = f => f.LandlordId == _command.LandlordId;
            var landlordFeedback = await _tenantFeedbackViaLandlordRepositoryMock.Object.Get(x=>true);
            landlordFeedback.Count().ShouldBe(3);
        }
    }
}
