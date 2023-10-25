using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class UpdateTenantContactInformationCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantContactInformation, Guid>> _tenantContactInformationRepositoryMock;
        private readonly Mock<ILogger<UpdateTenantContactInformationCommandHandler>> _loggerMock;
        private UpdateTenantContactInformationCommand _command;
        public UpdateTenantContactInformationCommandHandlerTest()
        {
            _tenantContactInformationRepositoryMock = MockTenantContactInformationRepository.GetTenantContactInformationRepository();
            _loggerMock = new Mock<ILogger<UpdateTenantContactInformationCommandHandler>>();
            _command = new UpdateTenantContactInformationCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                SelectedCountry = "CountyrTest",
                Province = "ProvinceTest",
                City = "CityTest",
                Email = "testymactest@test.test",
                PhoneNumber = new Totira.Bussiness.UserService.Commands.ContactInformationPhoneNumber("Test", "UpdateTest")
            };
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new UpdateTenantContactInformationCommandHandler(_loggerMock.Object,
                                                                           _tenantContactInformationRepositoryMock.Object);
            //var contextMock =new Mock<IContext>();

            //Act
            await handler.HandleAsync(null, _command);

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
            var handler = new UpdateTenantContactInformationCommandHandler(_loggerMock.Object,
                                                                           _tenantContactInformationRepositoryMock.Object);
            _command = new UpdateTenantContactInformationCommand
            {
                Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                SelectedCountry = "CountyrTest",
                Province = "ProvinceTest",
                City = "CityTest",
                Email = "testymactest@test.test",
                PhoneNumber = new Totira.Bussiness.UserService.Commands.ContactInformationPhoneNumber("Test", "UpdateTest")
            };
            //Act
            //await Assert.ThrowsAnyAsync<InvalidOperationException>(() => handler.HandleAsync(null, _command));

            //Assert

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var contactInfo = (await _tenantContactInformationRepositoryMock.Object.Get(x => x.TenantId == _command.TenantId)).FirstOrDefault();

            contactInfo.ShouldBeNull();

        }
    }
}
