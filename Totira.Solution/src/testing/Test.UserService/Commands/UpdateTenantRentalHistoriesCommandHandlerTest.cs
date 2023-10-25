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
    public class UpdateTenantRentalHistoriesCommandHandlerTest : BaseCommandHandlerTest<UpdateTenantRentalHistoriesCommand, UpdateTenantRentalHistoriesCommandHandler>
    {
        private readonly Mock<IRepository<TenantRentalHistories, Guid>> _tenantRentalHistoriesRepositoryMock;
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;
        private readonly Mock<IOtpHandler> _otpHandlerMock;
        private readonly Mock<IOptions<FrontendSettings>> _settingsMock;

        protected override UpdateTenantRentalHistoriesCommandHandler CommandHandler => new UpdateTenantRentalHistoriesCommandHandler(_tenantRentalHistoriesRepositoryMock.Object,
                                                                        _loggerMock.Object,
                                                                        _tenantPersonalInformationRepositoryMock.Object,
                                                                        _emailHandlerMock.Object,
                                                                        _settingsMock.Object,
                                                                        _contextFactoryMock.Object,
                                                                        _messageServiceMock.Object,
                                                                        _otpHandlerMock.Object);

        public UpdateTenantRentalHistoriesCommandHandlerTest()
            : base(new UpdateTenantRentalHistoriesCommand
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
            })
        {
            _tenantRentalHistoriesRepositoryMock = MockTenantRentalHistoriesRepository.GetTenantRentalHistoriesRepository();
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
            _otpHandlerMock = MockOtp.GetIOtpHandlerMock();
            _settingsMock = new Mock<IOptions<FrontendSettings>>();
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert
            var history = await _tenantRentalHistoriesRepositoryMock.Object.GetByIdAsync(_command.TenantId);
            history.RentalHistories.Count().ShouldBe(3);
        }

        [Fact]
        public async Task HandleAsyncTest_NonExistantId()
        {
            //Arrange
            var nonExistantItemCommand = new UpdateTenantRentalHistoriesCommand
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
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), nonExistantItemCommand);

            //Assert
            var history = await _tenantRentalHistoriesRepositoryMock.Object.GetByIdAsync(nonExistantItemCommand.TenantId);
           
            history.ShouldBeNull();

        }
    }
}
