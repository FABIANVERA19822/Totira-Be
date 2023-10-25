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
    public class UpdateTenantApplicationDetailsCommandHandlerTest : BaseCommandHandlerTest<UpdateTenantApplicationDetailsCommand, UpdateTenantApplicationDetailsCommandHandler>
    {
        private readonly Mock<IRepository<TenantApplicationDetails, Guid>> _tenantApplicationDetailssRepositoryMock;
        private readonly Mock<IRepository<TenantVerificationProfile, Guid>> _tenantVerificationProfileMock;

        protected override UpdateTenantApplicationDetailsCommandHandler CommandHandler => new UpdateTenantApplicationDetailsCommandHandler(
            _tenantApplicationDetailssRepositoryMock.Object, _tenantVerificationProfileMock.Object, _loggerMock.Object, _contextFactoryMock.Object, _messageServiceMock.Object);

       
        public UpdateTenantApplicationDetailsCommandHandlerTest()
            : base(new UpdateTenantApplicationDetailsCommand
            {
                Id = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                EstimatedMove = new Totira.Bussiness.UserService.Commands.ApplicationDetailEstimatedMove(1, 10),
                EstimatedRent = "update test",
                Occupants = new Totira.Bussiness.UserService.Commands.ApplicationDetailOccupants(2, 2),                
                Smoker = true,
                Pets = new List<Totira.Bussiness.UserService.Commands.ApplicationDetailPet>()
                { new Totira.Bussiness.UserService.Commands.ApplicationDetailPet()
                    {
                        Type = "Crocodile",
                        Size = "Tiny",
                        Description = "Albino"
                    }
                },
                Cars = new List<Totira.Bussiness.UserService.Commands.ApplicationDetailCar>()
                {
                    new Totira.Bussiness.UserService.Commands.ApplicationDetailCar()
                    {
                        Model = "Test",
                        Make = "Test",
                        Plate = "Test",
                        Year = 1980
                    }
                },
                IsVerificationsRequested= false
            })
        {
            _tenantApplicationDetailssRepositoryMock = MockTenantApplicationDetailsRepository.GetTenantApplicationDetailsRepository();
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert
            var ApplicationInfo = await _tenantApplicationDetailssRepositoryMock.Object.GetByIdAsync(_command.Id);
            ApplicationInfo.EstimatedMove.Month.ShouldBe(_command.EstimatedMove.Month);
            ApplicationInfo.EstimatedRent.ShouldBe(_command.EstimatedRent);
            ApplicationInfo.Occupants.Adults.ShouldBe(_command.Occupants.Adults);
            ApplicationInfo.Smoker.ShouldBe(_command.Smoker);
            ApplicationInfo.Pets.Count.ShouldBe(_command.Pets.Count);
            ApplicationInfo.Cars.Count.ShouldBe(_command.Cars.Count);
        }

        [Fact]
        public async Task HandleAsyncTest_NonExistantId()
        {
            //Arrange
            var itemNonExistantecommand = new UpdateTenantApplicationDetailsCommand
            {
                Id = Guid.NewGuid(),
                EstimatedMove = new Totira.Bussiness.UserService.Commands.ApplicationDetailEstimatedMove(1, 10),
                EstimatedRent = "update test",
                Occupants = new Totira.Bussiness.UserService.Commands.ApplicationDetailOccupants(2, 2),
                Smoker = true,
                Pets = new List<Totira.Bussiness.UserService.Commands.ApplicationDetailPet>()
                { new Totira.Bussiness.UserService.Commands.ApplicationDetailPet()
                    {
                        Type = "Crocodile",
                        Size = "Tiny",
                        Description = "Albino"
                    }
                },
                Cars = new List<Totira.Bussiness.UserService.Commands.ApplicationDetailCar>()
                {
                    new Totira.Bussiness.UserService.Commands.ApplicationDetailCar()
                    {
                        Model = "Test",
                        Make = "Test",
                        Plate = "Test",
                        Year = 1980
                    }
                }
            };

            //Act
            var ApplicationInfo = await _tenantApplicationDetailssRepositoryMock.Object.GetByIdAsync(itemNonExistantecommand.Id);

            ApplicationInfo.ShouldBeNull();
            //Assert
            var referrals = await _tenantApplicationDetailssRepositoryMock.Object.Get(x => true);
            referrals.Count().ShouldBe(2);
        }
    }
}
