using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events.Landlord.UpdatedEvents;
using Totira.Bussiness.UserService.Handlers.Commands.Landlords.Update;
using Totira.Support.Application.Messages;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands;

public class CancelStatusPropertyApplicationCommandHandlerTest : BaseCommandHandlerTest<CancelStatusPropertyApplicationCommand, CancelStatusPropertyApplicationCommandHandler>
{
    private readonly Mock<IRepository<TenantPropertyApplication, Guid>> _tenantPropertyApplicationRepositoryMock;

    protected override CancelApproveOrRejectPropertyApplicationCommandHandlerToTest CommandHandler =>
        new(_tenantPropertyApplicationRepositoryMock.Object,
            _loggerMock.Object,
            _contextFactoryMock.Object,
            _messageServiceMock.Object);

    public CancelStatusPropertyApplicationCommandHandlerTest()
        : base(new()
        {
            PropertyApplicationId = Guid.Empty,
        })
    {
        _tenantPropertyApplicationRepositoryMock = MockTenantPropertyApplicationRepository.GetTenantPropertyApplicationRepository();
    }

    [Theory]
    [InlineData("c4a4b33f-30e3-41ba-8988-4e08c9361185")]
    [InlineData("8699d287-2ad2-4660-b686-4370191f9e2c")]
    public async Task Process_Should_CancelSuccessfully(string id)
    {
        //Arrange
        var context = new Mock<IContext>();
        context.SetupGet(x => x.Href).Returns("");
        context.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
        context.SetupGet(x => x.CreatedBy).Returns(new Guid("d00b5ccc-a6ac-4c1c-b8d0-75d6c73d91b4"));
        context.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);
        var propertyApplicationId = Guid.Parse(id);
        _command.PropertyApplicationId = propertyApplicationId;

        //Act
        var result = await CommandHandler.TestProcess(context.Object, _command);

        //Assert
        result.ShouldBeOfType(typeof(PropertyApplicationStatusCanceledEvent));
        result.IsValid.ShouldBeTrue();
        result.PropertyApplicationId.ShouldBe(propertyApplicationId);

        var entity = MockTenantPropertyApplicationRepository.Values.First(x => x.Id == propertyApplicationId);
        entity.Status.ShouldBe("Canceled");
        entity.UpdatedOn.ShouldNotBeNull();
    }

    [Theory]
    [InlineData("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")]
    public async Task Process_Should_ReturnError_When_StatusIsNotCorrect(string id)
    {
        //Arrange
        var context = new Mock<IContext>();
        context.SetupGet(x => x.Href).Returns("");
        context.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
        context.SetupGet(x => x.CreatedBy).Returns(new Guid("d00b5ccc-a6ac-4c1c-b8d0-75d6c73d91b4"));
        context.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);
        _command.PropertyApplicationId = Guid.Parse(id);

        //Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
        {
            await CommandHandler.TestProcess(context.Object, _command);
        });

        exception.Message.ShouldBe("Property application cannot be canceled because is not approved or rejected.");
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public async Task Process_Should_ReturnError_When_PropertyApplicationDoesNotExist(string id)
    {
        //Arrange
        var context = new Mock<IContext>();
        context.SetupGet(x => x.Href).Returns("");
        context.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
        context.SetupGet(x => x.CreatedBy).Returns(new Guid("d00b5ccc-a6ac-4c1c-b8d0-75d6c73d91b4"));
        context.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);
        _command.PropertyApplicationId = Guid.Parse(id);
        
        //Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
        {
            await CommandHandler.TestProcess(context.Object, _command);
        });

        exception.Message.ShouldBe("Tenant property application does not exist.");
    }
}

public class CancelApproveOrRejectPropertyApplicationCommandHandlerToTest : CancelStatusPropertyApplicationCommandHandler
{
    public CancelApproveOrRejectPropertyApplicationCommandHandlerToTest(
        IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository,
        ILogger<CancelStatusPropertyApplicationCommandHandler> logger,
        IContextFactory contextFactory,
        IMessageService messageService)
        : base(tenantPropertyApplicationRepository, logger, contextFactory, messageService)
    {
    }
    
    /// <summary>
    /// Use this method only for test project. Implements Process() protected method for testing.
    /// </summary>
    /// <param name="context">Context interface</param>
    /// <param name="command">Command request</param>
    /// <returns>Event result</returns>
    public async Task<PropertyApplicationStatusCanceledEvent> TestProcess(IContext context, CancelStatusPropertyApplicationCommand command)
        => await base.Process(context, command);
}