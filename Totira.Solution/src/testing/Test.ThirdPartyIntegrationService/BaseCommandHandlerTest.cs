using Microsoft.Extensions.Logging;
using Moq;
using Test.ThirdPartyIntegrationService.Mocks.FactoryMocks;
using Test.ThirdPartyIntegrationService.Mocks.ServicesMock;
using Totira.Support.Application.Commands;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;

namespace Test.ThirdPartyIntegrationService;

public abstract class BaseCommandHandlerTest<TCommand, TCommandHandler> where TCommand : ICommand where TCommandHandler : IMessageHandler<TCommand>
{
    protected readonly Mock<IContextFactory> _contextFactoryMock;
    protected readonly Mock<IMessageService> _messageServiceMock;
    protected readonly Mock<IEmailHandler> _emailHandlerMock;
    protected readonly Mock<IEncryptionHandler> _encryptionHandlerMock;
    protected readonly TCommand _command;
    protected readonly Mock<ILogger<TCommandHandler>> _loggerMock;

    protected abstract TCommandHandler CommandHandler { get; }

    protected BaseCommandHandlerTest(TCommand command)
    {
        _contextFactoryMock = MockIContextFactory.GetIContextFactoryMock();
        _messageServiceMock = MockIMessageService.GetIMessageServiceMock();
        _emailHandlerMock = new Mock<IEmailHandler>();
        _encryptionHandlerMock = new Mock<IEncryptionHandler>();
        _loggerMock = new Mock<ILogger<TCommandHandler>>();
        _command = command;

        _encryptionHandlerMock.Setup(x => x.EncryptString(It.IsAny<string>())).Returns<string>(x => x);
        _encryptionHandlerMock.Setup(x => x.DecryptString(It.IsAny<string>())).Returns<string>(x => x);
    }
}