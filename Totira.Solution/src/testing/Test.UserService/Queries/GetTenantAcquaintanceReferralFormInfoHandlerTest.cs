using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
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

        public GetTenantAcquaintanceReferralFormInfoHandlerTest()
        {
            _configuration = new Mock<IOptions<FrontendSettings>>();
            _tenantAcquaintanceReferralFormInfoRepositoryMock = MockTenantAcquaintanceReferralFormInfoRepository.GetTenantAcquaintanceReferralFormInfoRepository();
            _tenantAcquaintanceReferralsRepositoryMock = MockTenantAcquaintanceReferralsRepository.GetTenantAcquaintanceReferralsRepository();
            _loggerMock = new Mock<ILogger<CreateTenantAcquaintanceReferralCommandHandler>>();
        }

        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            var handler = new GetAcquaintanceReferralFormInfoByReferralIdQueryHandler(_configuration.Object,
                                                                                _loggerMock.Object,
                                                                                _tenantAcquaintanceReferralFormInfoRepositoryMock.Object,
                                                                                _tenantAcquaintanceReferralsRepositoryMock.Object, null
                                                                              );

            var query = new QueryAcquaintanceReferralFormInfoByReferralId(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")); // should be mocked data id

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
                                                                                _tenantAcquaintanceReferralsRepositoryMock.Object, null
                                                                              );

            var query = new QueryAcquaintanceReferralFormInfoByReferralId(Guid.NewGuid());

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetAcquaintanceReferralFormInfoDto>();
            result.ShouldBeNull();
        }
    }
}
