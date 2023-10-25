using Moq;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Commands;
using Test.UserService.Mocks.RepoMocks;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using Totira.Support.EventServiceBus;

namespace Test.UserService.Commands;

public class DeleteCosignersFromGroupApplicationCommandHandlerTest : BaseCommandHandlerTest<DeleteCosignersFromGroupApplicationCommand, DeleteCosignersFromGroupApplicationCommandHandler>
{
    private readonly Mock<IRepository<TenantApplicationRequest, Guid>> _tenantApplicationRequestRepository;
    private readonly Mock<IRepository<TenantVerificationProfile, Guid>> _tenantVerificationProfileMock;
    private readonly Mock<IEventBus> _eventBusMock;

    protected override DeleteCosignersFromGroupApplicationCommandHandler CommandHandler =>
        new DeleteCosignersFromGroupApplicationCommandHandler(
            _tenantApplicationRequestRepository.Object, _tenantVerificationProfileMock.Object, _loggerMock.Object, _contextFactoryMock.Object, _messageServiceMock.Object, _eventBusMock.Object
        );

    public DeleteCosignersFromGroupApplicationCommandHandlerTest()
        : base(new DeleteCosignersFromGroupApplicationCommand
            {
                CoSignerId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1E"),
                ApplicationRequestId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")
            })
    {
        _tenantApplicationRequestRepository = MockTenantApplicationRequestRepo.GetTenantApplicationRequestRepository();
    }

    [Fact]
    public async Task HandleAsyncTest_Ok()
    {

        //Arrange
        //Act
        await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

        //Assert
        var origin = await _tenantApplicationRequestRepository.Object.GetByIdAsync(_command.ApplicationRequestId);

        Assert.Null(origin?.Guarantor);
    }

    [Fact]
    public async Task HandleAsyncTest_TenantApplicationRequestDoesntExist()
    {
        //Arrange
        //Act
        await CommandHandler.HandleAsync(Mock.Of<IContext>(), new DeleteCosignersFromGroupApplicationCommand() { ApplicationRequestId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1E"), CoSignerId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1D") });

        //Assert
        var origin = await _tenantApplicationRequestRepository.Object.GetByIdAsync(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1E"));

        Assert.Null(origin);
    }

}

