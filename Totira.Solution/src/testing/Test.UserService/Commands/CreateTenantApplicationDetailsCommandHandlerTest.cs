using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantApplicationDetailsCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantApplicationDetails, Guid>> _tenantApplicationDetailssRepositoryMock;
        private readonly Mock<ILogger<CreateTenantApplicationDetailsCommandHandler>> _loggerMock;
        private readonly Mock<IContextFactory> _contextFactoryMock;
        private readonly Mock<IMessageService> _messageServiceMock;
        private readonly Totira.Bussiness.UserService.Commands.CreateTenantApplicationDetailsCommand _command;
        public CreateTenantApplicationDetailsCommandHandlerTest()
        {
            _tenantApplicationDetailssRepositoryMock = MockTenantApplicationDetailsRepository.GetTenantApplicationDetailsRepository();
            _loggerMock = new Mock<ILogger<CreateTenantApplicationDetailsCommandHandler>>();
            _contextFactoryMock = new Mock<IContextFactory>();
            _messageServiceMock = new Mock<IMessageService>();
            _command = new Totira.Bussiness.UserService.Commands.CreateTenantApplicationDetailsCommand
            {
                Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                EstimatedMove = new Totira.Bussiness.UserService.Commands.ApplicationDetailEstimatedMove(4, 2023),
                EstimatedRent = "0",
                Occupants = new Totira.Bussiness.UserService.Commands.ApplicationDetailOccupants(2, 1),
                Smoker = false,
                Pets = new List<Totira.Bussiness.UserService.Commands.ApplicationDetailPet>(),
                Cars = new List<Totira.Bussiness.UserService.Commands.ApplicationDetailCar>()
            };
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new CreateTenantApplicationDetailsCommandHandler(_tenantApplicationDetailssRepositoryMock.Object, 
                _loggerMock.Object, _contextFactoryMock.Object, _messageServiceMock.Object);
            //var contextMock =new Mock<IContext>();

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var referrals = await _tenantApplicationDetailssRepositoryMock.Object.Get(x => true);
            referrals.Count().ShouldBe(3);
        }

        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            //Arrange
            var handler = new CreateTenantApplicationDetailsCommandHandler(_tenantApplicationDetailssRepositoryMock.Object,
                _loggerMock.Object, _contextFactoryMock.Object, _messageServiceMock.Object);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            //var referrals = await _tenantApplicationDetailssRepositoryMock.Object.Get(x => true);
            //referrals.Count().ShouldBe(2);
        }
    }
}
