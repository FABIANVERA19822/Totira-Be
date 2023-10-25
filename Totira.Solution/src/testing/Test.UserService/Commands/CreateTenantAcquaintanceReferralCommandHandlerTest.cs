using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Test.UserService.Mocks.ObjectMocks;
using Test.UserService.Mocks.RepoMocks;
using Shouldly;
using Test.UserService.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantAcquaintanceReferralCommandHandlerTest
    {

        private readonly Mock<IRepository<TenantAcquaintanceReferrals,Guid>> _tenantAcquaintanceReferralsRepositoryMock;
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;
        private readonly Mock<IEmailHandler> _emailHandlerMock;
        private readonly Mock<IOptions<FrontendSettings>> _settingsMock;
        private readonly Mock<ILogger<CreateTenantAcquaintanceReferralCommandHandler>> _loggerMock;
        private readonly CreateTenantAcquaintanceReferralCommand _command;

        public CreateTenantAcquaintanceReferralCommandHandlerTest()
        {
            _tenantAcquaintanceReferralsRepositoryMock = MockTenantAcquaintanceReferralsRepository.GetTenantAcquaintanceReferralsRepository();
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
            _emailHandlerMock = new Mock<IEmailHandler>();
            _loggerMock = new Mock<ILogger<CreateTenantAcquaintanceReferralCommandHandler>>();
            _settingsMock = MockSettingsFrontEndOptionsRepository.GetSettingsRepository();
            _command = new CreateTenantAcquaintanceReferralCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                FullName = "Not Mocked Data",
                Email = "testyMacTest@test.test",
                Relationship = "",
                OtherRelationship = "",
                PhoneNumber = new Totira.Bussiness.UserService.Commands.ContactInformationPhoneNumber(string.Empty, string.Empty),
                Status = ""

            };
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new CreateTenantAcquaintanceReferralCommandHandler(
                tenantAcquaintanceReferralsRepository: _tenantAcquaintanceReferralsRepositoryMock.Object,
                tenantBasicInformationRepository: _tenantPersonalInformationRepositoryMock.Object,
                logger: _loggerMock.Object,
                emailHandler: _emailHandlerMock.Object,
                settings: _settingsMock.Object);

            var origin = (await _tenantAcquaintanceReferralsRepositoryMock.Object.Get(x => true)).Count();

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var referrals = (await _tenantAcquaintanceReferralsRepositoryMock.Object.Get(x => true)).Count();

            Assert.Equal(origin + 1, referrals);
        }

        [Fact]
        public async Task HandleAsyncTest_MissingId()
        {
         
            //Arrange
            var handler = new CreateTenantAcquaintanceReferralCommandHandler(
                tenantAcquaintanceReferralsRepository: _tenantAcquaintanceReferralsRepositoryMock.Object,
                tenantBasicInformationRepository: _tenantPersonalInformationRepositoryMock.Object,
                logger: _loggerMock.Object,
                emailHandler: _emailHandlerMock.Object,
                settings: _settingsMock.Object);

            var _commandMissingId = new CreateTenantAcquaintanceReferralCommand
            {
                TenantId = Guid.NewGuid(),
                FullName = "Not Mocked Data",
                Email = "testymactest@test.test",
                Relationship = "",
                OtherRelationship = "",
                PhoneNumber = new Totira.Bussiness.UserService.Commands.ContactInformationPhoneNumber(string.Empty, string.Empty),
                Status = ""
            };

            await Assert.ThrowsAnyAsync<Exception>(async () => await handler.HandleAsync(null, _commandMissingId));

        }
    }
}
