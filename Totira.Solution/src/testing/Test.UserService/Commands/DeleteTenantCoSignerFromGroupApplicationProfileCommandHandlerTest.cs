using Microsoft.Extensions.Logging;
using Moq;
using Test.UserService.Mocks.ObjectMocks;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.EventServiceBus;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands;

public class DeleteTenantCoSignerFromGroupApplicationProfileCommandHandlerTest
{

    private readonly Mock<ILogger<DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler>> _logger;
    private readonly Mock<IRepository<TenantApplicationRequest, Guid>> _tenantApplicationRequestRepository;
    private readonly Mock<IRepository<TenantGroupApplicationProfile, Guid>> _tenantGroupApplicationProfileRepository;



    private readonly DeleteTenantCoSignerFromGroupApplicationProfileCommand _command;

    public DeleteTenantCoSignerFromGroupApplicationProfileCommandHandlerTest(   )
    {
        _logger = new Mock<ILogger<DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler>>();
        _tenantApplicationRequestRepository = MockTenantApplicationRequestRepo.GetTenantApplicationRequestRepository();
        

        _command = new DeleteTenantCoSignerFromGroupApplicationProfileCommand {CoSignerId= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1E"), ApplicationRequestId= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C") };

    }

    [Fact]
    public async Task HandleAsyncTest_Ok()
    {

        //Arrange

        var handler = new DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler(_tenantApplicationRequestRepository.Object,  _logger.Object);
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

        var handler = new DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler(_tenantApplicationRequestRepository.Object,  _logger.Object);
        //Act
       await handler.HandleAsync(null, new DeleteTenantCoSignerFromGroupApplicationProfileCommand() {ApplicationRequestId= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1E"), CoSignerId= new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1D") });

        //Assert
        var origin = (await _tenantApplicationRequestRepository.Object.GetByIdAsync(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1E")));

        Assert.Null(origin);

    }

}
