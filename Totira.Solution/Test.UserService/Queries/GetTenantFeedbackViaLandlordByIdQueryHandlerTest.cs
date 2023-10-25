using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Test.UserService.RepoMocks;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries
{
    public class GetTenantFeedbackViaLandlordByIdQueryHandlerTest
    {
        private readonly Mock<IRepository<TenantFeedbackViaLandlord>> _tenantFeedbackViaLandlordRepositoryMock;
        private readonly Mock<IRepository<TenantRentalHistories>> _tenantRentalHistoriesRepositoryMock;
        private readonly Mock<ILogger<CreateTenantFeedbackViaLandlordCommandHandler>> _loggerMock;

        public GetTenantFeedbackViaLandlordByIdQueryHandlerTest()
        {
            _tenantFeedbackViaLandlordRepositoryMock = MockTenantFeedbackViaLandlordRepository.GetTenantFeedbackViaLandlordRepository();
            _tenantRentalHistoriesRepositoryMock = MockTenantRentalHistoriesRepository.GetTenantRentalHistoriesRepository();
            _loggerMock = new Mock<ILogger<CreateTenantFeedbackViaLandlordCommandHandler>>();
        }

        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            var handler = new GetTenantFeedbackViaLandlordByIdQueryHandler(_tenantFeedbackViaLandlordRepositoryMock.Object,
                                                                            _tenantRentalHistoriesRepositoryMock.Object,
                                                                            _loggerMock.Object);

            var query = new QueryTenantFeedbackViaLandlordById(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantFeedbackViaLandlordDto>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task HandleAsyncTest_BadId()
        {
            //Arrange
            var handler = new GetTenantFeedbackViaLandlordByIdQueryHandler(_tenantFeedbackViaLandlordRepositoryMock.Object,
                                                                            _tenantRentalHistoriesRepositoryMock.Object,
                                                                            _loggerMock.Object);

            var query = new QueryTenantFeedbackViaLandlordById(Guid.NewGuid());

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantFeedbackViaLandlordDto>();
            result.ShouldBeNull();
        }
    }
}
