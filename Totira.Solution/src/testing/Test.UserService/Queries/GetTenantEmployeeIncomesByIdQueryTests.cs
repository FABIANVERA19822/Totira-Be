using Moq;
using Shouldly;
using Test.UserService.Mocks.ObjectMocks;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries;

public class GetTenantEmployeeIncomesByIdQueryTests
{
    [Fact]
    public async Task HandleAsync_WhenExists_ShouldReturn_ValidData()
    {
        var repositoryMock = new Mock<IRepository<TenantEmployeeIncomes, Guid>>();
        
        repositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((TenantEmployeeIncomes)null!);
    }
    [Fact]
    public async Task HandleAsync_WhenStudentIsFalse_WithValidData_ReturnsEmployeeIncome()
    {
        //Arrange
        var repositoryMock = new Mock<IRepository<TenantEmployeeIncomes, Guid>>();
        var handler = new GetTenantEmployeeIncomesByIdQueryHandler(repositoryMock.Object);
        var query = new QueryTenantEmployeeIncomesById(Guid.NewGuid());
        var tenant = MockTenantEmployeeIncomes.GetEmployeeIncomes(query.TenantId);
        repositoryMock.Setup(x => x.GetByIdAsync(query.TenantId)).ReturnsAsync(tenant);

        //Act
        var result = await handler.HandleAsync(query);

        //Assert
        result.ShouldNotBeNull();
        result.TenantId.ShouldBeEquivalentTo(query.TenantId);
        result.IsStudent.ShouldBeFalse();
        result.CurrentEmployements.ShouldNotBeEmpty();
        result.PastEmployments.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task HandleAsync_WhenStudentIsTrue_WithValidData_ReturnsEmployeeIncomeAndStudentIncome()
    {
        //Arrange
        var repositoryMock = new Mock<IRepository<TenantEmployeeIncomes, Guid>>();
        var handler = new GetTenantEmployeeIncomesByIdQueryHandler(repositoryMock.Object);
        var query = new QueryTenantEmployeeIncomesById(Guid.NewGuid());
        var tenant = MockTenantEmployeeIncomes.GetStudentIncomes(query.TenantId);
        repositoryMock.Setup(x => x.GetByIdAsync(query.TenantId)).ReturnsAsync(tenant);

        //Act
        var result = await handler.HandleAsync(query);

        //Assert
        result.ShouldNotBeNull();
        result.TenantId.ShouldBeEquivalentTo(query.TenantId);
        result.IsStudent.ShouldBeTrue();
        result.CurrentEmployements.ShouldNotBeEmpty();
        result.PastEmployments.ShouldNotBeEmpty();
        result.StudyDetails.ShouldNotBeNull();
    }
}
