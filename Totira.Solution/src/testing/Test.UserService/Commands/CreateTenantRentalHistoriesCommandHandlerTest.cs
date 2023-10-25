using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Test.UserService.Mocks.OtpMocks;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.Otp;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantRentalHistoriesCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantRentalHistories, Guid>> _tenantRentalHistoriesRepositoryMock;
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;
        private readonly Mock<ILogger<CreateTenantRentalHistoriesCommandHandler>> _loggerMock;
        private readonly Mock<IEmailHandler> _emailHandlerMock;
        private readonly Mock<IOptions<FrontendSettings>> _settingsMock;
        private readonly Mock<IEncryptionHandler> _encryptionHandlerMock;
        private readonly CreateTenantRentalHistoriesCommand _command;
        private readonly Mock<IOtpHandler> _otpHandler;
        public CreateTenantRentalHistoriesCommandHandlerTest()
        {
            _tenantRentalHistoriesRepositoryMock = MockTenantRentalHistoriesRepository.GetTenantRentalHistoriesRepository();
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
            _loggerMock = new Mock<ILogger<CreateTenantRentalHistoriesCommandHandler>>();
            _emailHandlerMock = new Mock<IEmailHandler>();
            _settingsMock = new Mock<IOptions<FrontendSettings>>();
            _encryptionHandlerMock = new Mock<IEncryptionHandler>();
            _otpHandler = MockOtp.GetIOtpHandlerMock();
            _command = new CreateTenantRentalHistoriesCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                RentalStartDate = new Totira.Bussiness.UserService.Commands.CustomDate(04, 2022),
                CurrentlyLivingHere = false,
                RentalEndDate = new Totira.Bussiness.UserService.Commands.CustomDate(04, 2022),
                Country = "Argentina",
                State = "Misiones",
                City = "Posadas",
                Address = "Calle Falsa 123",
                Unit = "House",
                ContactInformation = new Totira.Bussiness.UserService.Commands.LandlordContactInformation
                {
                }
            };
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new CreateTenantRentalHistoriesCommandHandler(_loggerMock.Object,
                                                                        _tenantRentalHistoriesRepositoryMock.Object,
                                                                        _tenantPersonalInformationRepositoryMock.Object,
                                                                        _emailHandlerMock.Object,
                                                                        _settingsMock.Object,
                                                                        _otpHandler.Object);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var referrals = await _tenantRentalHistoriesRepositoryMock.Object.GetByIdAsync(_command.TenantId);
            referrals.RentalHistories.FirstOrDefault().Address.ShouldBe(_command.Address);
            referrals.RentalHistories.FirstOrDefault().City.ShouldBe(_command.City);
            referrals.RentalHistories.FirstOrDefault().Country.ShouldBe(_command.Country);
            referrals.RentalHistories.FirstOrDefault().CurrentlyLivingHere.ShouldBe(_command.CurrentlyLivingHere);
        }

        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            //Arrange
            var handler = new CreateTenantRentalHistoriesCommandHandler(_loggerMock.Object,
                                                                        _tenantRentalHistoriesRepositoryMock.Object,
                                                                        _tenantPersonalInformationRepositoryMock.Object,
                                                                        _emailHandlerMock.Object,
                                                                        _settingsMock.Object,
                                                                        _otpHandler.Object);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var referrals = await _tenantRentalHistoriesRepositoryMock.Object.Get(x => true);
            referrals.Count().ShouldBe(1);
        }
    }
}
