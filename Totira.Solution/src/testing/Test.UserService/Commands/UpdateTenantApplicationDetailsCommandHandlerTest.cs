using Microsoft.Extensions.Logging;
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
    public class UpdateTenantApplicationDetailsCommandHandlerTest
    {

        private readonly Mock<IRepository<TenantApplicationDetails, Guid>> _tenantApplicationDetailssRepositoryMock;
        private readonly Mock<ILogger<UpdateTenantBasicInformationCommandHandler>> _loggerMock;
        private readonly Mock<IContextFactory> _contextFactoryMock;
        private readonly Mock<IMessageService> _messageServiceMock;
        private UpdateTenantApplicationDetailsCommand _command;

        public UpdateTenantApplicationDetailsCommandHandlerTest()
        {
            _tenantApplicationDetailssRepositoryMock = MockTenantApplicationDetailsRepository.GetTenantApplicationDetailsRepository();
            _loggerMock = new Mock<ILogger<UpdateTenantBasicInformationCommandHandler>>();
            _contextFactoryMock = new Mock<IContextFactory>(MockBehavior.Strict);
            _messageServiceMock = new Mock<IMessageService>(MockBehavior.Strict);
            _command = new UpdateTenantApplicationDetailsCommand
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
            };
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new UpdateTenantApplicationDetailsCommandHandler(_tenantApplicationDetailssRepositoryMock.Object,
                _loggerMock.Object,
                _contextFactoryMock.Object,
                _messageServiceMock.Object);
            //var contextMock =new Mock<IContext>();

            //Act
            await handler.HandleAsync(null, _command);

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
            var handler = new UpdateTenantApplicationDetailsCommandHandler(_tenantApplicationDetailssRepositoryMock.Object,
                _loggerMock.Object,
                _contextFactoryMock.Object,
                _messageServiceMock.Object);
            _command = new UpdateTenantApplicationDetailsCommand
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
            var ApplicationInfo = await _tenantApplicationDetailssRepositoryMock.Object.GetByIdAsync(_command.Id);

            ApplicationInfo.ShouldBeNull();
            //Assert
            var referrals = await _tenantApplicationDetailssRepositoryMock.Object.Get(x => true);
            referrals.Count().ShouldBe(2);
        }
    }
}
