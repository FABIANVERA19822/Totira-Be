using Moq;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Commands;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using System.Linq.Expressions;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;

namespace Test.UserService.Commands
{
    public class CreateTenantApplicationTypeCommandHandlerTest : BaseCommandHandlerTest<CreateTenantApplicationTypeCommand, CreateTenantApplicationTypeCommandHandler>
    {        
        private readonly Mock<IRepository<TenantApplicationType, Guid>> _tenantApplicationTypeRepositoryMock;

        public CreateTenantApplicationTypeCommandHandlerTest()
            : base(new CreateTenantApplicationTypeCommand()
            {
                ApplicationType = "Group",
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")
            })
        {
            _tenantApplicationTypeRepositoryMock = MockTenantApplicationTypeRepository.GetTenantApplicationTypeRepository();
        }

        protected override CreateTenantApplicationTypeCommandHandler CommandHandler => new CreateTenantApplicationTypeCommandHandler(
                _loggerMock.Object,_tenantApplicationTypeRepositoryMock.Object, _contextFactoryMock.Object, _messageServiceMock.Object);

        [Fact]
        public async Task HandleAsyncTest_RequestOk_ReturnsOk()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert
            Expression<Func<TenantApplicationType, bool>> Expression = (ex => ex.TenantId == _command.TenantId);

            var tenantApplicationType = await _tenantApplicationTypeRepositoryMock.Object.Get(Expression);

            tenantApplicationType.FirstOrDefault().ApplicationType.ShouldBe(_command.ApplicationType);
        }
    }
}
