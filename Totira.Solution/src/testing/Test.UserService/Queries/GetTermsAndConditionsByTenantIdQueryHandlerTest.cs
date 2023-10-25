using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries;

public class GetTermsAndConditionsByTenantIdQueryHandlerTest
{
    [Fact]
    public async Task AcceptedTermsAndConditions_ShouldBe_True_When_EntityExists()
    {
        //Arrange
        var tenantId = Guid.NewGuid();
        var entity = new TenantTermsAndConditionsAcceptanceInfo
        {
            TenantId = tenantId,
            SigningDateTime = DateTime.UtcNow,
        };

        var repositoryMock = new Mock<IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid>>();
        var loggerMock = new Mock<ILogger<GetTermsAndConditionsByTenantIdQueryHandler>>();

        repositoryMock
            .Setup(x => x.Get(It.IsAny<Expression<Func<TenantTermsAndConditionsAcceptanceInfo, bool>>>()))
            .ReturnsAsync(new List<TenantTermsAndConditionsAcceptanceInfo>() { entity });

        var query = new QueryTermsAndConditionsByTenantId(tenantId);
        var handler = new GetTermsAndConditionsByTenantIdQueryHandler(
            repositoryMock.Object,
            loggerMock.Object);

        //Act
        var result = await handler.HandleAsync(query);

        //Assert
        result.TenantId.ShouldBe(tenantId);
        result.AcceptedTermsAndConditions.ShouldBeTrue();
    }

    [Fact]
    public async Task AcceptedTermsAndConditions_ShouldBe_False_When_EntityDoNotExists()
    {
        //Arrange
        var tenantId = Guid.NewGuid();

        var repositoryMock = new Mock<IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid>>();
        var loggerMock = new Mock<ILogger<GetTermsAndConditionsByTenantIdQueryHandler>>();

        repositoryMock
            .Setup(x => x.Get(It.IsAny<Expression<Func<TenantTermsAndConditionsAcceptanceInfo, bool>>>()))
            .ReturnsAsync(new List<TenantTermsAndConditionsAcceptanceInfo>());

        var query = new QueryTermsAndConditionsByTenantId(tenantId);
        var handler = new GetTermsAndConditionsByTenantIdQueryHandler(
            repositoryMock.Object,
            loggerMock.Object);

        //Act
        var result = await handler.HandleAsync(query);

        //Assert
        result.TenantId.ShouldBe(tenantId);
        result.AcceptedTermsAndConditions.ShouldBeFalse();
    }
}
