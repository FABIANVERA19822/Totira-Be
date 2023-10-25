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
    public class GetTenantAcquaintanceReferralFormInfoHandlerTest
    {
        private readonly Mock<IOptions<FrontendSettings>> _configuration;
        private readonly Mock<IRepository<TenantAcquaintanceReferralFormInfo, Guid>> _tenantAcquaintanceReferralFormInfoRepositoryMock;
        private readonly Mock<IRepository<TenantAcquaintanceReferrals, Guid>> _tenantAcquaintanceReferralsRepositoryMock;
        private readonly Mock<ILogger<CreateTenantAcquaintanceReferralCommandHandler>> _loggerMock;
        private readonly Mock<ICommonFunctions> _commonFunctionsMock;

        public GetTenantAcquaintanceReferralFormInfoHandlerTest()
        {
            _configuration = new Mock<IOptions<FrontendSettings>>();
            _tenantAcquaintanceReferralFormInfoRepositoryMock = MockTenantAcquaintanceReferralFormInfoRepository.GetTenantAcquaintanceReferralFormInfoRepository();
            _tenantAcquaintanceReferralsRepositoryMock = MockTenantAcquaintanceReferralsRepository.GetTenantAcquaintanceReferralsRepository();
            _loggerMock = new Mock<ILogger<CreateTenantAcquaintanceReferralCommandHandler>>();
            _commonFunctionsMock = new Mock<ICommonFunctions>();
        }

        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            _commonFunctionsMock.Setup(x => x.GetProfilePhoto(It.IsAny<QueryTenantProfileImageById>()))
                .ReturnsAsync(new GetTenantProfileImageDto(Guid.NewGuid(),
                    new GetTenantProfileImageDto.ProfileImageFile("test", "application/jpeg", "https://test.test")
                ));
            var handler = new GetAcquaintanceReferralFormInfoByReferralIdQueryHandler(_configuration.Object,
                                                                                _loggerMock.Object,
                                                                                _tenantAcquaintanceReferralFormInfoRepositoryMock.Object,
                                                                                _tenantAcquaintanceReferralsRepositoryMock.Object, _commonFunctionsMock.Object
                                                                              );

            var query = new QueryAcquaintanceReferralFormInfoByReferralId(new Guid("75CC87A6-EF71-491C-BECE-CA3C5FE1DB94")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetAcquaintanceReferralFormInfoDto>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task HandleAsyncTest_BadId()
        {
            //Arrange
            var handler = new GetAcquaintanceReferralFormInfoByReferralIdQueryHandler(_configuration.Object,
                                                                                _loggerMock.Object,
                                                                                _tenantAcquaintanceReferralFormInfoRepositoryMock.Object,
                                                                                _tenantAcquaintanceReferralsRepositoryMock.Object,
                                                                                _commonFunctionsMock.Object
                                                                              );

            var query = new QueryAcquaintanceReferralFormInfoByReferralId(Guid.NewGuid());

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeNull();
        }
    }
}
