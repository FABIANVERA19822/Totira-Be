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
    public class UpdateTenantBasicInformationCommandHandlerTest : BaseCommandHandlerTest<UpdateTenantBasicInformationCommand, UpdateTenantBasicInformationCommandHandler>
    {
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantBasicInformationRepositoryMock;

        protected override UpdateTenantBasicInformationCommandHandler CommandHandler => new UpdateTenantBasicInformationCommandHandler(_tenantBasicInformationRepositoryMock.Object, _loggerMock.Object,
                _contextFactoryMock.Object, _messageServiceMock.Object);

        public UpdateTenantBasicInformationCommandHandlerTest()
            : base(new UpdateTenantBasicInformationCommand
            {
                Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                FirstName = "UpdateTesty",
                LastName = "UpdateTest",
                Birthday = new BasicInformationBirthday { Year = 1994, Day = 28, Month = 04 },
                AboutMe = "i'm me, update"
            })
        {
            _tenantBasicInformationRepositoryMock = MockTenantBasicInformationRepository.GetTenantBasicInformationRepository();
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert
            var basicInfo = await _tenantBasicInformationRepositoryMock.Object.GetByIdAsync(_command.Id);
            basicInfo.FirstName.ShouldBe(_command.FirstName);
            basicInfo.LastName.ShouldBe(_command.LastName);
            basicInfo.Birthday.Month.ShouldBe(_command.Birthday.Month);
            basicInfo.AboutMe.ShouldBe(_command.AboutMe);
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
            var basicInfo = await _tenantBasicInformationRepositoryMock.Object.GetByIdAsync(nonExistantItemCommand.Id);


            basicInfo.ShouldBeNull();
        }
    }
}
