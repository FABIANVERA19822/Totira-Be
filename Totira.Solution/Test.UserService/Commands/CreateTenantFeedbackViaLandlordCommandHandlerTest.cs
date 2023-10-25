using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Test.UserService.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantFeedbackViaLandlordCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantFeedbackViaLandlord>> _tenantFeedbackViaLandlordRepositoryMock;
        private readonly Mock<IRepository<TenantRentalHistories>> _tenantRentalHistoriesRepositoryMock;
        private readonly Mock<ILogger<CreateTenantFeedbackViaLandlordCommandHandler>> _loggerMock;
        private CreateTenantFeedbackViaLandlordCommand _command;

        public CreateTenantFeedbackViaLandlordCommandHandlerTest()
        {
            _tenantFeedbackViaLandlordRepositoryMock = MockTenantFeedbackViaLandlordRepository.GetTenantFeedbackViaLandlordRepository();
            _tenantRentalHistoriesRepositoryMock = MockTenantRentalHistoriesRepository.GetTenantRentalHistoriesRepository();
            _loggerMock = new Mock<ILogger<CreateTenantFeedbackViaLandlordCommandHandler>>();
            _command = new CreateTenantFeedbackViaLandlordCommand
            {
                TenantId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                LandlordId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
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
                                                                            _loggerMock.Object);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var landlordFeedback = await _tenantFeedbackViaLandlordRepositoryMock.Object.GetByIdAsync(_command.LandlordId);
            landlordFeedback.Score.ShouldBe(_command.StarScore);
        }

        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            //Arrange
            var handler = new CreateTenantFeedbackViaLandlordCommandHandler(_tenantFeedbackViaLandlordRepositoryMock.Object,
                                                                            _tenantRentalHistoriesRepositoryMock.Object,
                                                                            _loggerMock.Object);
            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var landlordFeedback = await _tenantFeedbackViaLandlordRepositoryMock.Object.Get(x => x.LandlordId == _command.LandlordId);
            landlordFeedback.Count().ShouldBe(1);
        }
    }
}
