using Microsoft.Extensions.Logging;
using Moq;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Commands;
using Test.UserService.Mocks.RepoMocks;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;

namespace Test.UserService.Commands;

public class DeleteCosignersFromGroupApplicationCommandHandlerTest
{
    private readonly Mock<ILogger<DeleteCosignersFromGroupApplicationCommandHandler>> _logger;
    private readonly Mock<IRepository<TenantApplicationRequest, Guid>> _tenantApplicationRequestRepository;
    private readonly Mock<IContextFactory> _contextFactoryMock;
    private readonly Mock<IMessageService> _messageServiceMock;


    private readonly DeleteCosignersFromGroupApplicationCommand _command;

    public DeleteCosignersFromGroupApplicationCommandHandlerTest()
    {
        _logger = new Mock<ILogger<DeleteCosignersFromGroupApplicationCommandHandler>>();
        _tenantApplicationRequestRepository = MockTenantApplicationRequestRepo.GetTenantApplicationRequestRepository();
        _contextFactoryMock = new Mock<IContextFactory>();
        _messageServiceMock = new Mock<IMessageService>();

        _command = new DeleteCosignersFromGroupApplicationCommand { CoSignerId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1E"), ApplicationRequestId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C") };

    }

    [Fact]
    public async Task HandleAsyncTest_Ok()
    {

        //Arrange

        var handler = new DeleteCosignersFromGroupApplicationCommandHandler(_tenantApplicationRequestRepository.Object,
            _logger.Object, _contextFactoryMock.Object, _messageServiceMock.Object);
        //Act
        await handler.HandleAsync(null, _command);

        //Assert
        var origin = (await _tenantApplicationRequestRepository.Object.GetByIdAsync(_command.ApplicationRequestId));

        Assert.Null(origin?.Guarantor);

    }

    [Fact]
    public async Task HandleAsyncTest_TenantApplicationRequestDoesntExist()
    {
        //Arrange

        var handler = new DeleteCosignersFromGroupApplicationCommandHandler(_tenantApplicationRequestRepository.Object,
            _logger.Object, _contextFactoryMock.Object, _messageServiceMock.Object);
        //Act
        await handler.HandleAsync(null, new DeleteCosignersFromGroupApplicationCommand() { ApplicationRequestId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1E"), CoSignerId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1D") });

        //Assert
        var origin = (await _tenantApplicationRequestRepository.Object.GetByIdAsync(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1E")));

        Assert.Null(origin);

    }

}

