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
    public class UpdateTenantRentalHistoriesCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantRentalHistories, Guid>> _tenantRentalHistoriesRepositoryMock;
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;
        private readonly Mock<ILogger<UpdateTenantRentalHistoriesCommandHandler>> _loggerMock;
        private readonly Mock<IEmailHandler> _emailHandlerMock;
        private readonly Mock<IOptions<FrontendSettings>> _settingsMock;
        private readonly Mock<IContextFactory> _contextFactoryMock;
        private readonly Mock<IMessageService> _messageServiceMock;
        private UpdateTenantRentalHistoriesCommand _command;
        private readonly Mock<IOtpHandler> _otpHandler;
        public UpdateTenantRentalHistoriesCommandHandlerTest()
        {
            _tenantRentalHistoriesRepositoryMock = MockTenantRentalHistoriesRepository.GetTenantRentalHistoriesRepository();
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
            _loggerMock = new Mock<ILogger<UpdateTenantRentalHistoriesCommandHandler>>();
            _emailHandlerMock = new Mock<IEmailHandler>();
            _settingsMock = new Mock<IOptions<FrontendSettings>>();
            _otpHandler = MockOtp.GetIOtpHandlerMock();
            _contextFactoryMock = new Mock<IContextFactory>();
            _messageServiceMock = new Mock<IMessageService>();
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new UpdateTenantRentalHistoriesCommandHandler(_tenantRentalHistoriesRepositoryMock.Object,
                                                                        _loggerMock.Object,
                                                                        _tenantPersonalInformationRepositoryMock.Object,
                                                                        _emailHandlerMock.Object,
                                                                        _settingsMock.Object,
                                                                        _contextFactoryMock.Object,
                                                                        _messageServiceMock.Object,
                                                                        _otpHandler.Object);
            _command = new UpdateTenantRentalHistoriesCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                RentalStartDate = new Totira.Bussiness.UserService.Commands.CustomDate(05, 2023),
                CurrentlyLivingHere = true,
                RentalEndDate = new Totira.Bussiness.UserService.Commands.CustomDate(05, 2024),
                Country = "San Marino Update",
                State = "Castello di Faetano Update",
                City = "Faetano Update",
                Address = "Calle Falsa 123 Update",
                Unit = "House Update",
                ContactInformation = new Totira.Bussiness.UserService.Commands.LandlordContactInformation()
                {
                    Relationship = "update test",
                    FirstName = "update test",
                    LastName = "update test",
                    PhoneNumber = new Totira.Bussiness.UserService.Commands.RentalHistoriesPhoneNumber { CountryCode = "Test", Number = "UpdateTest" },
                    EmailAddress = "updatetest@test.test"
                }
            };

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var history = await _tenantRentalHistoriesRepositoryMock.Object.GetByIdAsync(_command.TenantId);
            history.RentalHistories.Count().ShouldBe(3);
        }

        [Fact]
        public async Task HandleAsyncTest_NonExistantId()
        {
            //Arrange
            var handler = new UpdateTenantRentalHistoriesCommandHandler(_tenantRentalHistoriesRepositoryMock.Object,
                                                            _loggerMock.Object,
                                                            _tenantPersonalInformationRepositoryMock.Object,
                                                            _emailHandlerMock.Object,
                                                            _settingsMock.Object,
                                                            _contextFactoryMock.Object,
                                                            _messageServiceMock.Object,
                                                            _otpHandler.Object);
            _command = new UpdateTenantRentalHistoriesCommand
            {
                TenantId = Guid.NewGuid(),
                RentalStartDate = new Totira.Bussiness.UserService.Commands.CustomDate(05, 2023),
                CurrentlyLivingHere = true,
                RentalEndDate = new Totira.Bussiness.UserService.Commands.CustomDate(05, 2024),
                Country = "San Marino Update",
                State = "Castello di Faetano Update",
                City = "Faetano Update",
                Address = "Calle Falsa 123 Update",
                Unit = "House Update",
                ContactInformation = new Totira.Bussiness.UserService.Commands.LandlordContactInformation()
                {
                    Relationship = "update test",
                    FirstName = "update test",
                    LastName = "update test",
                    PhoneNumber = new Totira.Bussiness.UserService.Commands.RentalHistoriesPhoneNumber { CountryCode = "Test", Number = "UpdateTest" },
                    EmailAddress = "updatetest@test.test"
                }
            };
           
            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var history = await _tenantRentalHistoriesRepositoryMock.Object.GetByIdAsync(_command.TenantId);
           
            history.ShouldBeNull();

        }
    }
}
