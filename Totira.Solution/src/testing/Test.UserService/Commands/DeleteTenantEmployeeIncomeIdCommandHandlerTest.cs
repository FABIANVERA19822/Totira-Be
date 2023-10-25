using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Test.UserService.Mocks.ObjectMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class DeleteTenantEmployeeIncomeIdCommandHandlerTest
    {

        private readonly Mock<IS3Handler> _s3HandlerMock;
        private readonly Mock<IRepository<TenantEmployeeIncomes, Guid>> _incomesTenantEmployeerepositoryMock;
        private readonly Mock<IRepository<TenantEmploymentReference, Guid>> _tenantEmploymentReferenceRepositoryMock; 
        private readonly Mock<ILogger<DeleteTenantEmployeeIncomeIdCommandHandler>> _loggerMock;
        private readonly DeleteTenantEmployeeIncomeIdCommand _command;

        public DeleteTenantEmployeeIncomeIdCommandHandlerTest()
        {
            _s3HandlerMock = new Mock<IS3Handler>();
            _incomesTenantEmployeerepositoryMock = new Mock<IRepository<TenantEmployeeIncomes, Guid>>();
            _tenantEmploymentReferenceRepositoryMock = new Mock<IRepository<TenantEmploymentReference, Guid>>();
            _loggerMock = new Mock<ILogger<DeleteTenantEmployeeIncomeIdCommandHandler>>();

            _command = new DeleteTenantEmployeeIncomeIdCommand(Guid.NewGuid(), Guid.NewGuid());

        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var tenantMock = MockTenantEmployeeIncomes.GetEmployeeIncomes(_command.TenantId);
            _incomesTenantEmployeerepositoryMock.Setup(x => x.GetByIdAsync(_command.TenantId)).ReturnsAsync(tenantMock);
            _s3HandlerMock.Setup(x => x.DeleteObjectAsync(It.IsAny<string>())).ReturnsAsync(true);

            var handler = new DeleteTenantEmployeeIncomeIdCommandHandler(_incomesTenantEmployeerepositoryMock.Object, _tenantEmploymentReferenceRepositoryMock.Object, _loggerMock.Object, _s3HandlerMock.Object);
           
            //Act 
            await handler.HandleAsync(null, _command);

            //Assert 
            var employeeIncomes = await _incomesTenantEmployeerepositoryMock.Object.GetByIdAsync(_command.TenantId); 
            var income = employeeIncomes.EmployeeIncomes!=null ?  employeeIncomes.EmployeeIncomes.FirstOrDefault(income => income.Id == _command.IncomeId) : null;
            var files = income != null ? income.Files : null;
            Assert.Null(files);
            Assert.Null(income); 

        }

        [Fact]
        public async Task HandleAsyncTest_EmployeeIncomesDoesntExist()
        {
            //Arrange
            var handler = new DeleteTenantEmployeeIncomeIdCommandHandler(_incomesTenantEmployeerepositoryMock.Object, _tenantEmploymentReferenceRepositoryMock.Object, _loggerMock.Object, _s3HandlerMock.Object);

            //Act 
            await handler.HandleAsync(null, _command);

            //Assert 

            var employeeIncomes = await _incomesTenantEmployeerepositoryMock.Object.GetByIdAsync(_command.TenantId);
            Assert.Null(employeeIncomes);

        }
         
    }

      
}
