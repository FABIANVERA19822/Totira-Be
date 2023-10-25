
using Microsoft.Extensions.Logging;
using Moq;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Commands;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using System.Linq.Expressions;

namespace Test.UserService.Commands
{
    public class CreateTenantApplicationTypeCommandHandlerTest
    {        
        private readonly Mock<IRepository<TenantApplicationType, Guid>> _tenantApplicationTypeRepositoryMock;
        private readonly Mock<ILogger<CreateTenantApplicationTypeCommandHandler>> _loggerMock;
        private readonly CreateTenantApplicationTypeCommand _command;

        public CreateTenantApplicationTypeCommandHandlerTest()
        {
            _tenantApplicationTypeRepositoryMock = MockTenantApplicationTypeRepository.GetTenantApplicationTypeRepository();
            _loggerMock = new Mock<ILogger<CreateTenantApplicationTypeCommandHandler>>();
            _command = new CreateTenantApplicationTypeCommand()
            {
                ApplicationType = "Group",
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")
            };
        }

        [Fact]
        public async Task HandleAsyncTest_RequestOk_ReturnsOk()
        {
            //Arrange
            var handler = new CreateTenantApplicationTypeCommandHandler(_loggerMock.Object,_tenantApplicationTypeRepositoryMock.Object);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            Expression<Func<TenantApplicationType, bool>> Expression = (ex => ex.TenantId == _command.TenantId);

            var tenantApplicationType = await _tenantApplicationTypeRepositoryMock.Object.Get(Expression);

            tenantApplicationType.FirstOrDefault().ApplicationType.ShouldBe(_command.ApplicationType);
        }
    }
}
