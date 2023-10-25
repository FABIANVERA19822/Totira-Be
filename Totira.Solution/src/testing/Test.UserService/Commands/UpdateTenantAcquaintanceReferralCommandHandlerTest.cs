using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System.Linq.Expressions;
using Test.UserService.Mocks.OtpMocks;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.Otp;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class UpdateTenantAcquaintanceReferralCommandHandlerTest : BaseCommandHandlerTest<UpdateTenantAcquaintanceReferralCommand, UpdateTenantAcquaintanceReferralCommandHandler>
    {
        private readonly Mock<IRepository<TenantAcquaintanceReferrals, Guid>> _tenantAcquaintanceReferralsRepositoryMock;
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantBasicInformationRepositoryMock;
        private readonly Mock<IOtpHandler> _otpHandlerMock;

        private readonly Mock<IOptions<FrontendSettings>> _settingsMock;

        protected override UpdateTenantAcquaintanceReferralCommandHandler CommandHandler => new UpdateTenantAcquaintanceReferralCommandHandler(
                tenantAcquaintanceReferralsRepository: _tenantAcquaintanceReferralsRepositoryMock.Object,
                tenantBasicInformationRepository: _tenantBasicInformationRepositoryMock.Object,
                otpHandler: _otpHandlerMock.Object,
                logger: _loggerMock.Object,
                emailHandler: _emailHandlerMock.Object,
                settings: _settingsMock.Object,
                contextFactory: _contextFactoryMock.Object,
                messageService: _messageServiceMock.Object);

        public UpdateTenantAcquaintanceReferralCommandHandlerTest()
            : base(new UpdateTenantAcquaintanceReferralCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                FullName = "Update Test",
                Email = "newEmail@test.test",
                Phone = new ContactInformationPhoneNumber("950266835", "+51"),
                Status = "updateTest",
                Relationship = "updateRelationship"
            })
        {
            _tenantAcquaintanceReferralsRepositoryMock = MockTenantAcquaintanceReferralsRepository.GetTenantAcquaintanceReferralsRepository();
            _tenantBasicInformationRepositoryMock = MockTenantBasicInformationRepository.GetTenantBasicInformationRepository();
            _otpHandlerMock = MockOtp.GetIOtpHandlerMock();
            _settingsMock = new Mock<IOptions<FrontendSettings>>();
        }

        [Fact]
        public async Task HandleAsyncTest_RequestOk_ReturnsOk()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert
            var referral = await _tenantAcquaintanceReferralsRepositoryMock.Object.GetByIdAsync(_command.TenantId);
            referral.Referrals.Where(x => x.FullName == _command.FullName).FirstOrDefault().Email.ShouldBe(_command.Email);
            referral.Referrals.Where(x => x.FullName == _command.FullName).FirstOrDefault().Phone.Number.ShouldBe(_command.Phone.Number);
            referral.Referrals.Where(x => x.FullName == _command.FullName).FirstOrDefault().Phone.CountryCode.ShouldBe(_command.Phone.CountryCode);
            referral.Referrals.Where(x => x.FullName == _command.FullName).FirstOrDefault().FullName.ShouldBe(_command.FullName);
            referral.Referrals.Where(x => x.FullName == _command.FullName).FirstOrDefault().Relationship.ShouldBe(_command.Relationship);
        }

        [Fact]
        public async Task HandleAsyncTest_NonExistantId_DoesntUpdateAnyEntity()
        {
            //Arrange

            var _commandNoExistantId = new UpdateTenantAcquaintanceReferralCommand
            {
                TenantId = Guid.NewGuid(),
                FullName = "Update Test",
                Email = "newEmail@test.test",
                Phone = new Totira.Bussiness.UserService.Commands.ContactInformationPhoneNumber("", ""),
                Status = "updateTest",
                Relationship = "updateRelationship"
            };
            //Act 
            await CommandHandler.HandleAsync(null, _commandNoExistantId);

            //Assert 
            Expression<Func<TenantAcquaintanceReferrals, bool>> expression = p => p.Referrals.Where(x => x.Email == _commandNoExistantId.Email).Count()>0;

            var tenantAcquaintanceReferrals = await _tenantAcquaintanceReferralsRepositoryMock.Object.Get(expression);
            tenantAcquaintanceReferrals.Count().ShouldBe(0);
        }
    }
}