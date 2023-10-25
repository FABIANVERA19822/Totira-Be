using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantApplicationDetailsCommandHandlerTest : BaseCommandHandlerTest<CreateTenantApplicationDetailsCommand, CreateTenantApplicationDetailsCommandHandler>
    {
        private readonly Mock<IRepository<TenantApplicationDetails, Guid>> _tenantApplicationDetailssRepositoryMock;

        public CreateTenantApplicationDetailsCommandHandlerTest()
            : base(new CreateTenantApplicationDetailsCommand
            {
                Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                EstimatedMove = new Totira.Bussiness.UserService.Commands.ApplicationDetailEstimatedMove(4, 2023),
                EstimatedRent = "0",
                Occupants = new Totira.Bussiness.UserService.Commands.ApplicationDetailOccupants(2, 1),
                Smoker = false,
                Pets = new List<Totira.Bussiness.UserService.Commands.ApplicationDetailPet>(),
                Cars = new List<Totira.Bussiness.UserService.Commands.ApplicationDetailCar>()
            })
        {
            _tenantApplicationDetailssRepositoryMock = MockTenantApplicationDetailsRepository.GetTenantApplicationDetailsRepository();
        }

        protected override CreateTenantApplicationDetailsCommandHandler CommandHandler => new CreateTenantApplicationDetailsCommandHandler(
                _tenantApplicationDetailssRepositoryMock.Object, _loggerMock.Object, _contextFactoryMock.Object, _messageServiceMock.Object);

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            //var contextMock =new Mock<IContext>();

            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert
            var referrals = await _tenantApplicationDetailssRepositoryMock.Object.Get(x => true);
            referrals.Count().ShouldBe(3);
        }

        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert
            //var referrals = await _tenantApplicationDetailssRepositoryMock.Object.Get(x => true);
            //referrals.Count().ShouldBe(2);
        }
    }
}
