using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class UpdateTenantContactInformationCommandHandlerTest : BaseCommandHandlerTest<UpdateTenantContactInformationCommand, UpdateTenantContactInformationCommandHandler>
    {
        private readonly Mock<IRepository<TenantContactInformation, Guid>> _tenantContactInformationRepositoryMock;
        //private readonly Mock<ILogger<UpdateTenantContactInformationCommandHandler>> _loggerMock;
        //private readonly Mock<IContextFactory> _contextFactoryMock;
        //private readonly Mock<IMessageService> _messageServiceMock;
        private UpdateTenantContactInformationCommand _command;

        protected override UpdateTenantContactInformationCommandHandler CommandHandler => 
                       new UpdateTenantContactInformationCommandHandler(_loggerMock.Object,
                                                                        _tenantContactInformationRepositoryMock.Object,
                                                                        _contextFactoryMock.Object,
                                                                        _messageServiceMock.Object);

        public UpdateTenantContactInformationCommandHandlerTest()
            :base (new UpdateTenantContactInformationCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                SelectedCountry = "CountyrTest",
                Province = "ProvinceTest",
                City = "CityTest",
                Email = "testymactest@test.test",
                PhoneNumber = new ContactInformationPhoneNumber("Test", "UpdateTest")
            })
        {
            _tenantContactInformationRepositoryMock = MockTenantContactInformationRepository.GetTenantContactInformationRepository();
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var _command = new UpdateTenantContactInformationCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                SelectedCountry = "CountyrTest",
                Province = "ProvinceTest",
                City = "CityTest",
                Email = "testymactest@test.test",
                PhoneNumber = new ContactInformationPhoneNumber("Test", "UpdateTest")
            };
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert
            var contactInfo = (await _tenantContactInformationRepositoryMock.Object.Get(x => x.TenantId == _command.TenantId)).FirstOrDefault();
            contactInfo.PhoneNumber.Number.ShouldBe(_command.PhoneNumber.Number);
            contactInfo.PhoneNumber.CountryCode.ShouldBe(_command.PhoneNumber.CountryCode);
            contactInfo.Email.ShouldBe(_command.Email);
        }

        [Fact]
        public async Task HandleAsyncTest_NonExistantId()
        {
            //Arrange
            var _command = new UpdateTenantContactInformationCommand
            {
                Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1F"),
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1F"),
                SelectedCountry = "CountyrTest",
                Province = "ProvinceTest",
                City = "CityTest",
                Email = "testymactest@test.test",
                PhoneNumber = new ContactInformationPhoneNumber("Test", "UpdateTest")
            };
            //Act
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => CommandHandler.HandleAsync(Mock.Of<IContext>(), _command));


            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert
            var contactInfo = (await _tenantContactInformationRepositoryMock.Object.Get(x => x.TenantId == _command.TenantId)).FirstOrDefault();

            contactInfo.ShouldBeNull();

        }
    }
}
