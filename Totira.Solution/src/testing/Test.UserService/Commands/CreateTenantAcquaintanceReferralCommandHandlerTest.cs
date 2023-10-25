using LanguageExt;
using LanguageExt.ClassInstances;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Test.UserService.Mocks.OtpMocks;
using Test.UserService.Mocks.RepoMocks;
using Test.UserService.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.Otp;
using Xunit.Sdk;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantAcquaintanceReferralCommandHandlerTest : BaseCommandHandlerTest<CreateTenantAcquaintanceReferralCommand, CreateTenantAcquaintanceReferralCommandHandler>
    {

        private readonly Mock<IRepository<TenantAcquaintanceReferrals,Guid>> _tenantAcquaintanceReferralsRepositoryMock;
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;
        private readonly Mock<IOptions<FrontendSettings>> _settingsMock;
        private readonly Mock<IOtpHandler> _otpHandler;


        protected override CreateTenantAcquaintanceReferralCommandHandler CommandHandler => new CreateTenantAcquaintanceReferralCommandHandler(
                tenantAcquaintanceReferralsRepository: _tenantAcquaintanceReferralsRepositoryMock.Object,
                tenantBasicInformationRepository: _tenantPersonalInformationRepositoryMock.Object,
                logger: _loggerMock.Object,
                emailHandler: _emailHandlerMock.Object,
                otpHandler: _otpHandler.Object,
                settings: _settingsMock.Object);
        public CreateTenantAcquaintanceReferralCommandHandlerTest()
            : base(new CreateTenantAcquaintanceReferralCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                FullName = "Not Mocked Data",
                Email = "testyMacTest@test.test",
                Relationship = "",
                OtherRelationship = "",
                PhoneNumber = new ContactInformationPhoneNumber(string.Empty, string.Empty),
                Status = ""

            })
        {
            _tenantAcquaintanceReferralsRepositoryMock = MockTenantAcquaintanceReferralsRepository.GetTenantAcquaintanceReferralsRepository();
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
            _otpHandler = MockOtp.GetIOtpHandlerMock();
            _settingsMock = MockSettingsFrontEndOptionsRepository.GetSettingsRepository();
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var origin = (await _tenantAcquaintanceReferralsRepositoryMock.Object.Get(x => true)).Count();

            //Act
            await CommandHandler.HandleAsync(null, _command);

            //Assert
            var referrals = (await _tenantAcquaintanceReferralsRepositoryMock.Object.Get(x => true)).Count();

            Assert.Equal(origin + 1, referrals);
        }

        [Fact]
        public async Task HandleAsyncTest_MissingId_DoesntCreateNewEntity()
        {


            //Arrange
            var _commandMissingId = new CreateTenantAcquaintanceReferralCommand
            {
                TenantId = Guid.NewGuid(),
                FullName = "Not Mocked Data",
                Email = "testymactest@test.test",
                Relationship = "",
                OtherRelationship = "",
                PhoneNumber = new Totira.Bussiness.UserService.Commands.ContactInformationPhoneNumber(string.Empty, string.Empty),
                Status = "",
        
            };

            //Act
            await CommandHandler.HandleAsync(null, _commandMissingId);

            //Assert
            var tenantAcquaintanceReferrals = await _tenantAcquaintanceReferralsRepositoryMock.Object.Get(x => true);
            tenantAcquaintanceReferrals.Count().ShouldBe(2);
        }
    }
}
