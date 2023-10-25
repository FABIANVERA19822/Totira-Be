using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Application.Messages;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class UpdateTenantPersonalInformationCommandHandlerTest : BaseCommandHandlerTest<UpdateTenantBasicInformationCommand, UpdateTenantBasicInformationCommandHandler>
    {
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;

        protected override UpdateTenantBasicInformationCommandHandler CommandHandler => new UpdateTenantBasicInformationCommandHandler(_tenantPersonalInformationRepositoryMock.Object, _loggerMock.Object,
                _contextFactoryMock.Object, _messageServiceMock.Object);

        public UpdateTenantPersonalInformationCommandHandlerTest()
        : base(new UpdateTenantBasicInformationCommand
            {
                Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                FirstName = "UpdateTesty",
                LastName = "UpdateTest",
                Birthday = new BasicInformationBirthday { Year = 1994, Day = 28, Month = 04 },
                AboutMe = "i'm me, update"
            })
        {
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

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
            var nonExistantItemCommand = new UpdateTenantBasicInformationCommand
            {
                Id = Guid.NewGuid(),
                FirstName = "UpdateTesty",
                LastName = "UpdateTest",
                Birthday = new BasicInformationBirthday { Year = 1994, Day = 28, Month = 04 },
                AboutMe = "i'm me, update"
            };
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), nonExistantItemCommand);

            //Assert
            var personalInfo = await _tenantPersonalInformationRepositoryMock.Object.GetByIdAsync(nonExistantItemCommand.Id);
            personalInfo.ShouldBeNull();
        }
    }
}
