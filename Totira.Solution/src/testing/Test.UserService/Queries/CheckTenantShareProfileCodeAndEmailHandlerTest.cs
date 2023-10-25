using Microsoft.Extensions.DependencyModel;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries;

public class CheckTenantShareProfileCodeAndEmailHandlerTest
{
    private readonly Mock<IRepository<TenantShareProfile, Guid>> _tenantShareProfilesRepositoryMock;
    private readonly Mock<Totira.Support.CommonLibrary.CommonlHandlers.EncryptionHandler> _encryptionHandler;

    public CheckTenantShareProfileCodeAndEmailHandlerTest()
    {
        _tenantShareProfilesRepositoryMock = MockTenantShareProfileRepository.GetTenantShareProfileRepository();
        _encryptionHandler = new Mock<Totira.Support.CommonLibrary.CommonlHandlers.EncryptionHandler>();
    }
    [Fact]
    public async Task HandleAsyncTest_Ok()
    {

        //Arrange
        var handler = new CheckTenantShareProfileCodeAndEmailHandler(_tenantShareProfilesRepositoryMock.Object, _encryptionHandler.Object);
        var query = new QueryTenantShareProfileForCheckCodeAndEmail(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"), 971970, "test12@test.com");

        //Act
        var result = await handler.HandleAsync(query);

        //Assert
        result.ShouldBeOfType<GetTenantShareProfileForCheckCodeAndEmailDto>();
        result.IsSuccess.ShouldBe(true);
        result.Id.ShouldBe(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1D"));
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task HandleAsyncTest_BadId()
    {
        //Arrange
        var handler = new CheckTenantShareProfileCodeAndEmailHandler(_tenantShareProfilesRepositoryMock.Object, _encryptionHandler.Object);
        var query = new QueryTenantShareProfileForCheckCodeAndEmail(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"), 97195, "test12@test.com");

        //Act
        var result = await handler.HandleAsync(query);

        //Assert
        result.ShouldBeOfType<GetTenantShareProfileForCheckCodeAndEmailDto>();
        result.IsSuccess.ShouldBe(false);
        result.Id.ShouldBe(Guid.Empty);
        result.ErrorMessage.ShouldBe("Invalid credentials. Please try again");
    }
}
