using Amazon.Runtime.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Test.UserService.Mocks.OtpMocks;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.Otp;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class UpdateTenantAcquaintanceReferralCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantAcquaintanceReferrals, Guid>> _tenantAcquaintanceReferralsRepositoryMock;
        private readonly Mock<ILogger<CreateTenantAcquaintanceReferralCommandHandler>> _loggerMock;
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;
        private readonly Mock<IEmailHandler> _emailHandlerMock;
        private readonly Mock<IOptions<FrontendSettings>> _settingsMock;
        private readonly Mock<IEncryptionHandler> _encryptionHandlerMock;
        private readonly Mock<IContextFactory> _contextFactoryMock;
        private readonly Mock<IMessageService> _messageServiceMock;
        private readonly Mock<IOtpHandler> _otpHandler;
        private UpdateTenantAcquaintanceReferralCommand _command;

        public UpdateTenantAcquaintanceReferralCommandHandlerTest()
        {
            _tenantAcquaintanceReferralsRepositoryMock = MockTenantAcquaintanceReferralsRepository.GetTenantAcquaintanceReferralsRepository();
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
            _emailHandlerMock = new Mock<IEmailHandler>();
            _loggerMock = new Mock<ILogger<CreateTenantAcquaintanceReferralCommandHandler>>();
            _settingsMock = new Mock<IOptions<FrontendSettings>>();

            _otpHandler = MockOtp.GetIOtpHandlerMock();

            _command = new UpdateTenantAcquaintanceReferralCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                FullName = "Update Test",
                Email = "newEmail@test.test",
                Phone = new Totira.Bussiness.UserService.Commands.ContactInformationPhoneNumber("950266835", "+51"),
                Status = "updateTest",
                Relationship = "updateRelationship"
            };


        }
        [Fact]
        public async Task HandleAsyncTest_RequestOk_ReturnsOk()
        {
            //Arrange
            var handler = new UpdateTenantAcquaintanceReferralCommandHandler(
                tenantAcquaintanceReferralsRepository: _tenantAcquaintanceReferralsRepositoryMock.Object,
                logger: _loggerMock.Object,
                tenantPersonalInformationRepository: _tenantPersonalInformationRepositoryMock.Object,
                emailHandler: _emailHandlerMock.Object,
                settings: _settingsMock.Object,
                encryptionHandler: _encryptionHandlerMock.Object,
                otpHandler: _otpHandler.Object,
                contextFactory: _contextFactoryMock.Object,
                messageService: _messageServiceMock.Object);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var referral = await _tenantAcquaintanceReferralsRepositoryMock.Object.GetByIdAsync(_command.TenantId);
            referral.Referrals.Where(x => x.FullName == _command.FullName).FirstOrDefault().Email.ShouldBe(_command.Email);
            referral.Referrals.Where(x => x.FullName == _command.FullName).FirstOrDefault().Phone.Number.ShouldBe(_command.Phone.Number);
            referral.Referrals.Where(x => x.FullName == _command.FullName).FirstOrDefault().Phone.CountryCode.ShouldBe(_command.Phone.CountryCode);
            referral.Referrals.Where(x => x.FullName == _command.FullName).FirstOrDefault().FullName.ShouldBe(_command.FullName);
            referral.Referrals.Where(x => x.FullName == _command.FullName).FirstOrDefault().Relationship.ShouldBe(_command.Relationship);
        }

        [Fact] 
        public async Task HandleAsyncTest_NonExistantId_ThrowsException()
        {
            //Arrange
            var handler = new UpdateTenantAcquaintanceReferralCommandHandler(
                tenantAcquaintanceReferralsRepository: _tenantAcquaintanceReferralsRepositoryMock.Object,
                logger: _loggerMock.Object,
                tenantPersonalInformationRepository: _tenantPersonalInformationRepositoryMock.Object,
                emailHandler: _emailHandlerMock.Object,
                settings: _settingsMock.Object,
                encryptionHandler: _encryptionHandlerMock.Object,
                otpHandler: _otpHandler.Object,
                contextFactory: _contextFactoryMock.Object,
                messageService: _messageServiceMock.Object);

          var  _commandNoExistantId = new UpdateTenantAcquaintanceReferralCommand
            {
                TenantId = Guid.NewGuid(),
                FullName = "Update Test",
                Email = "newEmail@test.test",
                Phone = new Totira.Bussiness.UserService.Commands.ContactInformationPhoneNumber("", ""),
                Status = "updateTest",
                Relationship = "updateRelationship"
            };
            //Act
            await Assert.ThrowsAnyAsync<NullReferenceException>(() => handler.HandleAsync(null, _commandNoExistantId));
        }
    }
}
