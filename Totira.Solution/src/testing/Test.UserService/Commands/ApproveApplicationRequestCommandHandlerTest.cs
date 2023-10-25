using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Bussiness.UserService.Handlers.Commands.Landlords.Update;
using Totira.Support.Application.Messages;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands;

public class ApproveApplicationRequestCommandHandlerTest : BaseCommandHandlerTest<ApproveApplicationRequestCommand, ApproveApplicationRequestCommandHandler>
{
    private readonly Mock<IRepository<TenantPropertyApplication, Guid>> _tenantPropertyApplicationRepository;
    protected override ApproveApplicationRequestCommandHandlerToTest CommandHandler => 
        new(_loggerMock.Object,
            _contextFactoryMock.Object,
            _messageServiceMock.Object,
            _tenantPropertyApplicationRepository.Object);
    
    public ApproveApplicationRequestCommandHandlerTest()
        : base(new()
        {
            PropertyId = "T1234567",
            TenantId = Guid.Parse("2c4091b8-c8dd-4763-bffb-49d92a6bfd1c"),
            ApplicationRequestId = Guid.Parse("a196a475-0e02-4825-b384-4cce54a789b2")
        })
    {
        _tenantPropertyApplicationRepository = MockTenantPropertyApplicationRepository.GetTenantPropertyApplicationRepository();
    }

    [Fact]
    public async Task Process_Should_ReturnsOk()
    {
        //Arrange
        var context = new Mock<IContext>();
        context.SetupGet(x => x.Href).Returns("");
        context.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
        context.SetupGet(x => x.CreatedBy).Returns(new Guid("d00b5ccc-a6ac-4c1c-b8d0-75d6c73d91b4"));
        context.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);

        //Act
        var result = await CommandHandler.TestProcess(context.Object, _command);

        //Assert
        result.ShouldBeOfType(typeof(ApplicationRequestApprovedEvent));
        result.IsValid.ShouldBeTrue();
        result.Id.ShouldBe(Guid.Parse("1538ae98-bc44-4496-995f-d9528ad1fefb"));

        var entity = MockTenantPropertyApplicationRepository.Values.First(x => x.Id == Guid.Parse("1538ae98-bc44-4496-995f-d9528ad1fefb"));
        entity.Status.ShouldBe("Approved");
        entity.UpdatedOn.ShouldNotBeNull();
    }

    [Fact]
    public async Task Process_Should_ReturnsError_When_PropertyApplicationDoesNotExist()
    {
        //Arrange
        var context = new Mock<IContext>();
        context.SetupGet(x => x.Href).Returns("");
        context.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
        context.SetupGet(x => x.CreatedBy).Returns(new Guid("d00b5ccc-a6ac-4c1c-b8d0-75d6c73d91b4"));
        context.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);
        _command.PropertyId = "T7654321";
        
        //Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
        {
            await CommandHandler.TestProcess(context.Object, _command);
        });

        exception.Message.ShouldBe("Property application does not exist.");
    }
}

public class ApproveApplicationRequestCommandHandlerToTest : ApproveApplicationRequestCommandHandler
{
    public ApproveApplicationRequestCommandHandlerToTest(
        ILogger logger,
        IContextFactory contextFactory,
        IMessageService messageService,
        IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository)
        : base(logger, contextFactory, messageService, tenantPropertyApplicationRepository)
    {
    }

    /// <summary>
    /// Use this method only for test project. Implements Process() protected method for testing.
    /// </summary>
    /// <param name="context">Context interface</param>
    /// <param name="command">Command request</param>
    /// <returns>Event result</returns>
    public async Task<ApplicationRequestApprovedEvent> TestProcess(IContext context, ApproveApplicationRequestCommand command)
        => await base.Process(context, command);
}