using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Test.UserService.Mocks.ObjectMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.EventServiceBus;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class DeleteTenantEmployeeIncomeIdCommandHandlerTest : BaseCommandHandlerTest<DeleteTenantEmployeeIncomeIdCommand, DeleteTenantEmployeeIncomeIdCommandHandler>
    {

        private readonly Mock<IS3Handler> _s3HandlerMock;
        private readonly Mock<IRepository<TenantEmployeeIncomes, Guid>> _incomesTenantEmployeerepositoryMock;
        private readonly Mock<IRepository<TenantEmploymentReference, Guid>> _tenantEmploymentReferenceRepositoryMock;
        private readonly Mock<IRepository<TenantApplicationDetails, Guid>> _repositoryApplicationDetailsMock;
        private readonly Mock<IRepository<TenantCurrentJobStatus, Guid>> _repositoryCurrentJobStatusMock;
        private readonly Mock<IRepository<TenantVerificationProfile, Guid>> _tenantVerificationProfileMock;

        protected override DeleteTenantEmployeeIncomeIdCommandHandler CommandHandler => 
            new DeleteTenantEmployeeIncomeIdCommandHandler(
                _incomesTenantEmployeerepositoryMock.Object,
                _tenantEmploymentReferenceRepositoryMock.Object,
                _repositoryApplicationDetailsMock.Object,
                _repositoryCurrentJobStatusMock.Object,
                _tenantVerificationProfileMock.Object,
                Mock.Of<IOptions<RestClientOptions>>(),
                Mock.Of<IQueryRestClient>(),
                _loggerMock.Object,
                _s3HandlerMock.Object,
                Mock.Of<IEventBus>(),
                _contextFactoryMock.Object,
                _messageServiceMock.Object);

        public DeleteTenantEmployeeIncomeIdCommandHandlerTest()
            : base(new DeleteTenantEmployeeIncomeIdCommand(Guid.NewGuid(), Guid.NewGuid()))
        {
            _s3HandlerMock = new Mock<IS3Handler>();
            _incomesTenantEmployeerepositoryMock = new Mock<IRepository<TenantEmployeeIncomes, Guid>>();
            _tenantEmploymentReferenceRepositoryMock = new Mock<IRepository<TenantEmploymentReference, Guid>>();
            _repositoryApplicationDetailsMock = new Mock<IRepository<TenantApplicationDetails, Guid>>();
            _repositoryCurrentJobStatusMock = new Mock<IRepository<TenantCurrentJobStatus, Guid>>();
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var tenantMock = MockTenantEmployeeIncomes.GetEmployeeIncomes(_command.TenantId);
            _incomesTenantEmployeerepositoryMock.Setup(x => x.GetByIdAsync(_command.TenantId)).ReturnsAsync(tenantMock);
            _s3HandlerMock.Setup(x => x.DeleteObjectAsync(It.IsAny<string>())).ReturnsAsync(true);
          
            //Act 
            await CommandHandler.HandleAsync(null, _command);

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
            //Act 
            await CommandHandler.HandleAsync(null, _command);

            //Assert 

            var employeeIncomes = await _incomesTenantEmployeerepositoryMock.Object.GetByIdAsync(_command.TenantId);
            Assert.Null(employeeIncomes);

        }
         
    }

      
}
