using Moq;
using Test.UserService.Mocks.ObjectMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.EventServiceBus;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands;

public  class DeleteTenantEmployeeIncomeFileCommandHandlerTest : BaseCommandHandlerTest<DeleteTenantEmployeeIncomeFileCommand, DeleteTenantEmployeeIncomeFileCommandHandler>
{
    private readonly Mock<IS3Handler> _s3HandlerMock;
    private readonly Mock<IRepository<TenantEmployeeIncomes, Guid>> _incomesTenantEmployeerepositoryMock;
    private readonly Mock<IRepository<TenantEmploymentReference, Guid>> _tenantEmploymentReferenceRepositoryMock;

    protected override DeleteTenantEmployeeIncomeFileCommandHandler CommandHandler => 
        new DeleteTenantEmployeeIncomeFileCommandHandler(_s3HandlerMock.Object,_incomesTenantEmployeerepositoryMock.Object, _loggerMock.Object, _contextFactoryMock.Object, _messageServiceMock.Object);

    public DeleteTenantEmployeeIncomeFileCommandHandlerTest()
        : base(new DeleteTenantEmployeeIncomeFileCommand(Guid.NewGuid(), Guid.NewGuid(), "test.pdf"))
    {
        _s3HandlerMock = new Mock<IS3Handler>();
        _incomesTenantEmployeerepositoryMock = new Mock<IRepository<TenantEmployeeIncomes, Guid>>();
        _tenantEmploymentReferenceRepositoryMock = new Mock<IRepository<TenantEmploymentReference, Guid>>();
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
        var income = employeeIncomes.EmployeeIncomes != null ? employeeIncomes.EmployeeIncomes.FirstOrDefault(income => income.Id == _command.IncomeId) : null;
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
