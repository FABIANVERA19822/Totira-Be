using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Test.UserService.Mocks.OtpMocks;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.Otp;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantShareProfileCommandHandlerTest : BaseCommandHandlerTest<CreateTenantShareProfileCommand, CreateTenantShareProfileCommandHandler>
    {

        private readonly Mock<IRepository<TenantShareProfile, Guid>> _tenantShareProfileRepositoryMock;
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;
        private readonly Mock<IOptions<FrontendSettings>> _settingsMock;
        private readonly Mock<IOtpHandler> _otpHandler;

        protected override CreateTenantShareProfileCommandHandler CommandHandler => new CreateTenantShareProfileCommandHandler(
               logger: _loggerMock.Object,
              tenantShareProfileRepository: _tenantShareProfileRepositoryMock.Object,
              tenantBasicInformationRepository: _tenantPersonalInformationRepositoryMock.Object,
              emailHandler: _emailHandlerMock.Object,
              settings: _settingsMock.Object,
              encryptionHandler: _encryptionHandlerMock.Object,
              otpHandler: _otpHandler.Object);

        public CreateTenantShareProfileCommandHandlerTest()
            : base(new CreateTenantShareProfileCommand
            {
                TenantId = Guid.NewGuid(),
                Email = "testymactest@test.test",
                Message = ""
            })
        {
            _tenantShareProfileRepositoryMock = MockTenantShareProfileRepository.GetTenantShareProfileRepository();
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
            _otpHandler = MockOtp.GetIOtpHandlerMock();
            _settingsMock = new Mock<IOptions<FrontendSettings>>();
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var origin = (await _tenantShareProfileRepositoryMock.Object.Get(x => true)).Count();

            //Act
            await CommandHandler.HandleAsync(null, _command);

            //Assert
            var _tenantShareProfileCount = (await _tenantShareProfileRepositoryMock.Object.Get(x => true)).Count();

            Assert.Equal(origin + 1, _tenantShareProfileCount);
        }

        [Fact]
        public async Task HandleAsyncTest_MissingId()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(null, _command);

            //Assert
            var _tenantShareProfile = await _tenantShareProfileRepositoryMock.Object.Get(x => true);
            _tenantShareProfile.Count().ShouldBe(3);
        }
    }
}
