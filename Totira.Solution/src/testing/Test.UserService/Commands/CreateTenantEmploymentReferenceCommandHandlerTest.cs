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
    public class CreateTenantEmploymentReferenceCommandHandlerTest : BaseCommandHandlerTest<CreateTenantEmploymentReferenceCommand, CreateTenantEmploymentReferenceCommandHandler>
    {
        private readonly Mock<IRepository<TenantEmploymentReference, Guid>> _tenantEmploymentReferenceRepositoryMock;

        public CreateTenantEmploymentReferenceCommandHandlerTest()
            : base(new CreateTenantEmploymentReferenceCommand
            {
                TenantId = new Guid("48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC"),
                FirstName = "Test1",
                LastName = "Test2",
                JobTitle = "Test3",
                Relationship = "Test4",
                Email = "Test5@test.test",
                PhoneNumber = new Totira.Bussiness.UserService.Commands.EmploymentReferencePhoneNumber { CountryCode = "002", Number = "new test number" },

            })
        {
            _tenantEmploymentReferenceRepositoryMock = MockTenantEmploymentReferenceRepository.GetTenantTenantEmploymentReferenceRepository();
        }

        protected override CreateTenantEmploymentReferenceCommandHandler CommandHandler => new CreateTenantEmploymentReferenceCommandHandler(
                _tenantEmploymentReferenceRepositoryMock.Object, _loggerMock.Object, _contextFactoryMock.Object, _messageServiceMock.Object);

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            //var contextMock =new Mock<IContext>();

            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert
            var employmentReferenceInfo = await _tenantEmploymentReferenceRepositoryMock.Object.GetByIdAsync(_command.TenantId);
            employmentReferenceInfo.FirstName.ShouldBe(_command.FirstName);
            employmentReferenceInfo.LastName.ShouldBe(_command.LastName);
            employmentReferenceInfo.JobTitle.ShouldBe(_command.JobTitle);
            employmentReferenceInfo.Relationship.ShouldBe(_command.Relationship);
            employmentReferenceInfo.Email.ShouldBe(_command.Email);
            employmentReferenceInfo.PhoneNumber.Number.ShouldBe(_command.PhoneNumber.Number);
            employmentReferenceInfo.PhoneNumber.CountryCode.ShouldBe(_command.PhoneNumber.CountryCode);

        }

        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            //Arrange
            //Act
            await CommandHandler.HandleAsync(Mock.Of<IContext>(), _command);

            //Assert
            var employmentReferenceList = await _tenantEmploymentReferenceRepositoryMock.Object.Get(x => true);
            employmentReferenceList.Count().ShouldBe(2);
        }
    }
}
