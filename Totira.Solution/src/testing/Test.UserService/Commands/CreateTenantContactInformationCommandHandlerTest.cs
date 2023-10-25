using Moq;
using Shouldly;
using System.Linq.Expressions;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantContactInformationCommandHandlerTest : BaseCommandHandlerTest<CreateTenantContactInformationCommand, CreateTenantContactInformationCommandHandler>
    {
        private readonly Mock<IRepository<TenantContactInformation, Guid>> _tenantContactInformationRepositoryMock;

        public CreateTenantContactInformationCommandHandlerTest()
            : base(new CreateTenantContactInformationCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                SelectedCountry = "CountyrTest",
                Province = "ProvinceTest",
                City = "CityTest",
                Email = "testymactest@test.test",
                PhoneNumber = new Totira.Bussiness.UserService.Commands.ContactInformationPhoneNumber("Test", "UpdateTest")
            })
        {
            _tenantContactInformationRepositoryMock = MockTenantContactInformationRepository.GetTenantContactInformationRepository();
        }

        protected override CreateTenantContactInformationCommandHandler CommandHandler => 
                       new CreateTenantContactInformationCommandHandler(_loggerMock.Object, 
                                                                        _tenantContactInformationRepositoryMock.Object);

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(null, _command);

            //Assert
            Expression<Func<TenantContactInformation, bool>> filter = f => f.TenantId == _command.TenantId;
            var basicInfo = (await _tenantContactInformationRepositoryMock.Object.Get(filter)).FirstOrDefault();
            basicInfo.PhoneNumber.Number.ShouldBe(_command.PhoneNumber.Number);
            basicInfo.PhoneNumber.CountryCode.ShouldBe(_command.PhoneNumber.CountryCode);
            basicInfo.Email.ShouldBe(_command.Email);
        }

        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(null, _command);

            //Assert
            //var referrals = await _tenantContactInformationRepositoryMock.Object.Get(x => true);
            //referrals.Count().ShouldBe(1);
        }
    }
}
