using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Moq;
using Totira.Bussiness.UserService.Commands;
using Test.UserService.Mocks.RepoMocks;
using Test.UserService.Mocks.ObjectMocks;
using Test.UserService.Mocks.ResponseMocks;
using Microsoft.Extensions.Options;
using Shouldly;

namespace Test.UserService.Commands;
public class CreateTenantPropertyApplicationCommandHandlerTest : BaseCommandHandlerTest<CreatePropertyToApplyCommand, CreateTenantPropertyApplicationCommandHandler>
{
    private readonly Mock<IRepository<TenantPropertyApplication, Guid>> _tenantPropertyApplicationMockRepository;
    private readonly Mock<IRepository<TenantApplicationRequest, Guid>> _tenantApplicationRequestMockRepository;
    private readonly Mock<IQueryRestClient> _queryRestClientMock;
    private readonly Mock<IOptions<RestClientOptions>> _restClientOptionsMock;

    protected override CreateTenantPropertyApplicationCommandHandler CommandHandler => new CreateTenantPropertyApplicationCommandHandler(
        _tenantPropertyApplicationMockRepository.Object,
        _tenantApplicationRequestMockRepository.Object,
        _loggerMock.Object, 
        _queryRestClientMock.Object,
        _restClientOptionsMock.Object, 
        _contextFactoryMock.Object,
        _messageServiceMock.Object);

    public CreateTenantPropertyApplicationCommandHandlerTest()
        : base(new CreatePropertyToApplyCommand
        { 
            ApplicantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1D"),
            ApplicationRequestId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
            PropertyId = "C523423"
        })
    {
        _queryRestClientMock =  MockQueryRestClient_GetPropertyDetailsDto.GetMockAcceptedIQueryRestClient();
        _restClientOptionsMock = MockRestClientOptions.GetIOptionsMock();
        _tenantApplicationRequestMockRepository = MockTenantApplicationRequestRepo.GetTenantApplicationRequestRepository();
        _tenantPropertyApplicationMockRepository = MockTenantPropertyApplicationRepository.GetTenantPropertyApplicationRepository();
    }

    [Fact]
    public async Task HandleAsyncTest_RequestOk_ReturnsOk()
    {
        //Arrange    
        //Act
        await CommandHandler.HandleAsync(null, _command);

        //Assert
        var TenantpropertyApplication = await _tenantPropertyApplicationMockRepository.Object.Get(x=>true);
        Assert.NotNull(TenantpropertyApplication);
        TenantpropertyApplication.Count().ShouldBe(2);
    }

    [Fact]
    public async Task HandleAsyncTest_RequestFail_ReturnsEmptyTenantPropertyApplicationOk()
    {
        //Arrange
       var _commandEmpty = new CreatePropertyToApplyCommand
        {
            ApplicantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1D"),
            ApplicationRequestId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
            PropertyId = "C523423"
        };
        //Act
        await CommandHandler.HandleAsync(null, _commandEmpty);

        //Assert
        var TenantpropertyApplication = await _tenantPropertyApplicationMockRepository.Object.Get(x => true);
        Assert.NotNull(TenantpropertyApplication);
        TenantpropertyApplication.Count().ShouldBe(2);
    }
}


