using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantShareProfileCommandHandlerTest
    {

        private readonly Mock<IRepository<TenantShareProfile, Guid>> _tenantShareProfileRepositoryMock;
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;
        private readonly Mock<IEmailHandler> _emailHandlerMock;
        private readonly Mock<IOptions<FrontendSettings>> _settingsMock;
        private readonly Mock<ILogger<CreateTenantShareProfileCommandHandler>> _loggerMock;
        private readonly Mock<IEncryptionHandler> _encryptionHandler;
        private readonly CreateTenantShareProfileCommand _command;
        public CreateTenantShareProfileCommandHandlerTest()
        {
            _tenantShareProfileRepositoryMock = MockTenantShareProfileRepository.GetTenantShareProfileRepository();
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
            _emailHandlerMock = new Mock<IEmailHandler>();
            _loggerMock = new Mock<ILogger<CreateTenantShareProfileCommandHandler>>();
            _encryptionHandler = new Mock<IEncryptionHandler>();
            _settingsMock = new Mock<IOptions<FrontendSettings>>();
            _command = new CreateTenantShareProfileCommand
            {
                TenantId = Guid.NewGuid(),
                Email = "testymactest@test.test",
                Message = ""
            };
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new CreateTenantShareProfileCommandHandler(
                logger: _loggerMock.Object,
               tenantShareProfileRepository: _tenantShareProfileRepositoryMock.Object,
               tenantBasicInformationRepository: _tenantPersonalInformationRepositoryMock.Object,
               emailHandler: _emailHandlerMock.Object,
               settings: _settingsMock.Object, _encryptionHandler.Object);

            var origin = (await _tenantShareProfileRepositoryMock.Object.Get(x => true)).Count();

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var _tenantShareProfileCount = (await _tenantShareProfileRepositoryMock.Object.Get(x => true)).Count();

            Assert.Equal(origin + 1, _tenantShareProfileCount);
        }

        [Fact]
        public async Task HandleAsyncTest_MissingId()
        {
            //Arrange
            var handler = new CreateTenantShareProfileCommandHandler(
                logger: _loggerMock.Object,
                tenantShareProfileRepository: _tenantShareProfileRepositoryMock.Object,
                tenantBasicInformationRepository: _tenantPersonalInformationRepositoryMock.Object,
                
                emailHandler: _emailHandlerMock.Object,
                settings: _settingsMock.Object, _encryptionHandler.Object);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var _tenantShareProfile = await _tenantShareProfileRepositoryMock.Object.Get(x => true);
            _tenantShareProfile.Count().ShouldBe(3);
        }
    }
}
