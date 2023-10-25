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
    public class UpdateTenantPersonalInformationCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;
        private readonly Mock<ILogger<UpdateTenantBasicInformationCommandHandler>> _loggerMock;
        private UpdateTenantBasicInformationCommand _command;
        public UpdateTenantPersonalInformationCommandHandlerTest()
        {
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
            _loggerMock = new Mock<ILogger<UpdateTenantBasicInformationCommandHandler>>();
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new UpdateTenantBasicInformationCommandHandler(_tenantPersonalInformationRepositoryMock.Object, _loggerMock.Object);
            _command = new UpdateTenantBasicInformationCommand
            {
                Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                FirstName = "UpdateTesty",
                LastName = "UpdateTest",
                Birthday = new BasicInformationBirthday { Year = 1994, Day = 28, Month = 04 },
                AboutMe = "i'm me, update"
            };

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var personalInfo = await _tenantPersonalInformationRepositoryMock.Object.GetByIdAsync(_command.Id);
            personalInfo.FirstName.ShouldBe(_command.FirstName);
            personalInfo.LastName.ShouldBe(_command.LastName);
            personalInfo.Birthday.Month.ShouldBe(_command.Birthday.Month);
            personalInfo.AboutMe.ShouldBe(_command.AboutMe);
        }

        [Fact]
        public async Task HandleAsyncTest_NonExistantId()
        {
            //Arrange
            var handler = new UpdateTenantBasicInformationCommandHandler(_tenantPersonalInformationRepositoryMock.Object, _loggerMock.Object);
            _command = new UpdateTenantBasicInformationCommand
            {
                Id = Guid.NewGuid(),
                FirstName = "UpdateTesty",
                LastName = "UpdateTest",
                Birthday = new BasicInformationBirthday { Year = 1994, Day = 28, Month = 04 },
                AboutMe = "i'm me, update"
            };
            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var personalInfo = await _tenantPersonalInformationRepositoryMock.Object.GetByIdAsync(_command.Id);
            personalInfo.ShouldBeNull();
        }
    }
}
