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
    public class CreateTenantBasicInformationCommandHandlerTest : BaseCommandHandlerTest<CreateTenantBasicInformationCommand, CreateTenantBasicInformationCommandHandler>
    {
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;

        public CreateTenantBasicInformationCommandHandlerTest()
            : base(new CreateTenantBasicInformationCommand
            {
                Id = Guid.NewGuid(),
                FirstName = "Testy",
                LastName = "MacTest The Third",
                Birthday = new BasicInformationBirthday { Year = 1993, Day = 27, Month = 03 },
                AboutMe = "i'm the third me"
            })
        {
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
        }

        protected override CreateTenantBasicInformationCommandHandler CommandHandler => 
            new CreateTenantBasicInformationCommandHandler(
                _tenantPersonalInformationRepositoryMock.Object, 
                _loggerMock.Object,
                _contextFactoryMock.Object,
                _messageServiceMock.Object);

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var context = new Mock<IContext>();
            context.SetupGet(x => x.Href).Returns("");
            context.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
            context.SetupGet(x => x.CreatedBy).Returns(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"));
            context.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);
            //Act
            await CommandHandler.HandleAsync(context.Object, _command);

            //Assert
            var personalInfo = await _tenantPersonalInformationRepositoryMock.Object.GetByIdAsync(_command.Id);
            personalInfo.FirstName.ShouldBe(_command.FirstName);
            personalInfo.LastName.ShouldBe(_command.LastName);
        }

        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            var context = new Mock<IContext>();
            context.SetupGet(x => x.Href).Returns("");
            context.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
            context.SetupGet(x => x.CreatedBy).Returns(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"));
            context.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);
            //Act
            await CommandHandler.HandleAsync(context.Object, _command);

            //Assert
            //var referrals = await _tenantPersonalInformationRepositoryMock.Object.Get(x => true);
            //referrals.Count().ShouldBe(1);
        }
    }
}
