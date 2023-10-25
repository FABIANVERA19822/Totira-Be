using Moq;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Commands;
using Test.UserService.Mocks.RepoMocks;
using Shouldly;
using System.Linq.Expressions;
using Totira.Support.Application.Messages;

namespace Test.UserService.Commands
{
    public class UpdateTenantApplicationTypeCommandHandlerTest : BaseCommandHandlerTest<UpdateTenantApplicationTypeCommand, UpdateTenantApplicationTypeCommandHandler>
    {
        private readonly Mock<IRepository<TenantApplicationType, Guid>> _tenantApplicationTypeRepositoryMock;
        private readonly Mock<IRepository<TenantApplicationRequest, Guid>> _tenantApplicationRequestRepositoryMock;
        private readonly Mock<IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid>> _tenantApplicationRequestCoapplicantsSendEmailsRepositoryMock;

        protected override UpdateTenantApplicationTypeCommandHandler CommandHandler => new UpdateTenantApplicationTypeCommandHandler(
            _loggerMock.Object, 
            _tenantApplicationTypeRepositoryMock.Object, 
            _tenantApplicationRequestRepositoryMock.Object, 
            _tenantApplicationRequestCoapplicantsSendEmailsRepositoryMock.Object,
            _contextFactoryMock.Object,
            _messageServiceMock.Object);

        public UpdateTenantApplicationTypeCommandHandlerTest()
            : base(new UpdateTenantApplicationTypeCommand()
            {
                ApplicationType = "Group",
                TenantId = new Guid("E471131F-60C0-46F6-A980-11A37BE97473")
            })
        {
            _tenantApplicationTypeRepositoryMock = MockTenantApplicationTypeRepository.GetTenantApplicationTypeRepository();
            _tenantApplicationRequestRepositoryMock = new Mock<IRepository<TenantApplicationRequest, Guid>>();
            _tenantApplicationRequestCoapplicantsSendEmailsRepositoryMock = new Mock<IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid>>();
        }

        [Fact]
        public async Task HandleAsyncTest_RequestOk_ReturnsOk()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert 
            var tenantApplicationType = (await _tenantApplicationTypeRepositoryMock.Object.Get(x => x.TenantId == _command.TenantId)).FirstOrDefault();
            tenantApplicationType.ApplicationType.ShouldBe(_command.ApplicationType);
        }

        [Fact]
        public async Task HandleAsyncTest_NonExistantId_ReturnsNull()
        {
            //Arrange
            var _commandNoExistantId = new UpdateTenantApplicationTypeCommand
            {
                ApplicationType = "Group",
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")
            };
 
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _commandNoExistantId);

            //Assert

            Expression<Func<TenantApplicationType, bool>> expression = (r => r.TenantId == _commandNoExistantId.TenantId);
            var tenantApplicationType = (await _tenantApplicationTypeRepositoryMock.Object.Get(expression)).FirstOrDefault();
            tenantApplicationType.ShouldBeNull(); 
        }
    }
}
