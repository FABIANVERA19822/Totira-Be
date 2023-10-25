
namespace Test.UserService.Commands
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using static Totira.Support.Persistance.IRepository;
    using Totira.Bussiness.UserService.Domain;
    using Totira.Bussiness.UserService.Handlers.Commands;
    using Totira.Bussiness.UserService.Commands;
    using Test.UserService.Mocks.RepoMocks;
    using Shouldly;
    using System.Linq.Expressions;

    public class UpdateTenantApplicationTypeCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantApplicationType, Guid>> _tenantApplicationTypeRepositoryMock;
        private readonly Mock<ILogger<UpdateTenantApplicationTypeCommandHandler>> _loggerMock;
        private readonly UpdateTenantApplicationTypeCommand _command;

        public UpdateTenantApplicationTypeCommandHandlerTest()
        {
            _tenantApplicationTypeRepositoryMock = MockTenantApplicationTypeRepository.GetTenantApplicationTypeRepository();
            _loggerMock = new Mock<ILogger<UpdateTenantApplicationTypeCommandHandler>>();
            _command = new UpdateTenantApplicationTypeCommand()
            {
                ApplicationType = "Group",
                TenantId = new Guid("E471131F-60C0-46F6-A980-11A37BE97473")
            };
        }

        [Fact]
        public async Task HandleAsyncTest_RequestOk_ReturnsOk()
        {
            //Arrange
            var handler = new UpdateTenantApplicationTypeCommandHandler(_loggerMock.Object, _tenantApplicationTypeRepositoryMock.Object);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert 
            var tenantApplicationType = (await _tenantApplicationTypeRepositoryMock.Object.Get(x => x.TenantId == _command.TenantId)).FirstOrDefault();
            tenantApplicationType.ApplicationType.ShouldBe(_command.ApplicationType);
        }

        [Fact]
        public async Task HandleAsyncTest_NonExistantId_ReturnsNull()
        {
            //Arrange
            var handler = new UpdateTenantApplicationTypeCommandHandler(_loggerMock.Object, _tenantApplicationTypeRepositoryMock.Object);

            var _commandNoExistantId = new UpdateTenantApplicationTypeCommand
            {
                ApplicationType = "Group",
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")
            };
 
            //Act
            await handler.HandleAsync(null, _commandNoExistantId);

            //Assert

            Expression<Func<TenantApplicationType, bool>> expression = (r => r.TenantId == _commandNoExistantId.TenantId);
            var tenantApplicationType = (await _tenantApplicationTypeRepositoryMock.Object.Get(expression)).FirstOrDefault();
            tenantApplicationType.ShouldBeNull(); 
        }
    }
}
