using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.CommonLibrary.Settings;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries
{
    public class GetTenantFeedbackViaLandlordByIdQueryHandlerTest
    {
        private readonly Mock<IOptions<FrontendSettings>> _configuration;
        private readonly Mock<IRepository<TenantFeedbackViaLandlord, Guid>> _tenantFeedbackViaLandlordRepositoryMock;
        private readonly Mock<IRepository<TenantRentalHistories, Guid>> _tenantRentalHistoriesRepositoryMock;
        private readonly Mock<ILogger<CreateTenantFeedbackViaLandlordCommandHandler>> _loggerMock;
        private readonly Mock<ICommonFunctions> _commonFunctionsMock;

        public GetTenantFeedbackViaLandlordByIdQueryHandlerTest()
        {
            _configuration = new Mock<IOptions<FrontendSettings>>();
            _tenantFeedbackViaLandlordRepositoryMock = MockTenantFeedbackViaLandlordRepository.GetTenantFeedbackViaLandlordRepository();
            _tenantRentalHistoriesRepositoryMock = MockTenantRentalHistoriesRepository.GetTenantRentalHistoriesRepository();
            _loggerMock = new Mock<ILogger<CreateTenantFeedbackViaLandlordCommandHandler>>();
            _commonFunctionsMock = new Mock<ICommonFunctions>();

            _configuration.Setup(x => x.Value).Returns(new FrontendSettings
            {
                timeoutExpirationDaysRequest = 1
            });
        }

        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            _commonFunctionsMock.Setup(x => x.GetProfilePhoto(It.IsAny<QueryTenantProfileImageById>()))
                .ReturnsAsync(new GetTenantProfileImageDto(Guid.NewGuid(),
                    new GetTenantProfileImageDto.ProfileImageFile("test", "application/jpeg", "https://test.test")
                ));
            var handler = new GetTenantFeedbackViaLandlordByIdQueryHandler(_configuration.Object,
                                                                            _tenantFeedbackViaLandlordRepositoryMock.Object,
                                                                            _tenantRentalHistoriesRepositoryMock.Object,
                                                                            _loggerMock.Object,
                                                                            _commonFunctionsMock.Object);

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
            var handler = new GetTenantFeedbackViaLandlordByIdQueryHandler(_configuration.Object,
                                                                            _tenantFeedbackViaLandlordRepositoryMock.Object,
                                                                            _tenantRentalHistoriesRepositoryMock.Object,
                                                                            _loggerMock.Object,
                                                                            _commonFunctionsMock.Object);

            var query = new QueryTenantFeedbackViaLandlordById(Guid.NewGuid());

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeNull();
        }
    }
}
