using System;
using Moq;
using Test.UserService.Mocks.ObjectMocks;
using Test.UserService.Mocks.RepoMocks;
using Test.UserService.Mocks.ResponseMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using static Totira.Support.Persistance.IRepository;
using Shouldly;
namespace Test.UserService.Commands
{
	public class CreateTenantContactLandlordCommandHandlerTest : BaseCommandHandlerTest<CreateTenantContactLandlordCommand, CreateTenantContactLandlordCommandHandler>
    {
        private readonly Mock<IRepository<TenantContactInformation, Guid>> _tenantContactInformationRepositoryMock;
        private readonly Mock<IRepository<TenantContactLandlord, Guid>> _tenantContactLandlordRepository;

        protected override CreateTenantContactLandlordCommandHandler CommandHandler => new CreateTenantContactLandlordCommandHandler(
            _tenantContactLandlordRepository.Object,
            _loggerMock.Object,
            _contextFactoryMock.Object,
            _messageServiceMock.Object,
          _tenantContactInformationRepositoryMock.Object);

        public CreateTenantContactLandlordCommandHandlerTest()
     : base(new CreateTenantContactLandlordCommand
     {
         TenantId = new Guid("1538ae98-bc44-4496-995f-d9528ad1fefb"),
         PropertyId = "C523423",
      
     })
        {
            _tenantContactLandlordRepository = MockTenantContactLandlordRepository.GetTenantContactLandlordRepository();
            _tenantContactInformationRepositoryMock = MockTenantContactInformationRepository.GetTenantContactInformationRepository();
        }

        [Fact]
        public async Task HandleAsyncTest_RequestOk_ReturnsOk()
        {
            //Arrange    
            //Act
            await CommandHandler.HandleAsync(null, _command);

            //Assert
            var tenantContactLandlord = await _tenantContactLandlordRepository.Object.Get(x => true);
            Assert.NotNull(tenantContactLandlord);
            tenantContactLandlord.Count().ShouldBe(2);
        }


        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(null, _command);

            var tenantContactLandlord = await _tenantContactLandlordRepository.Object.Get(x => true);
            Assert.NotNull(tenantContactLandlord);
            tenantContactLandlord.Count().ShouldBe(2);
        }

    }
}

