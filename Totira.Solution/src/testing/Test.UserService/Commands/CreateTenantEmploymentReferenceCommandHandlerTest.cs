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
    public class CreateTenantEmploymentReferenceCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantEmploymentReference, Guid>> _tenantEmploymentReferenceRepositoryMock;
        private readonly Mock<ILogger<CreateTenantEmploymentReferenceCommandHandler>> _loggerMock;
        private readonly Mock<IContextFactory> _contextFactoryMock;
        private readonly Mock<IMessageService> _messageServiceMock;
        private readonly CreateTenantEmploymentReferenceCommand _command;
        public CreateTenantEmploymentReferenceCommandHandlerTest()
        {
            _tenantEmploymentReferenceRepositoryMock = MockTenantEmploymentReferenceRepository.GetTenantTenantEmploymentReferenceRepository();
            _loggerMock = new Mock<ILogger<CreateTenantEmploymentReferenceCommandHandler>>();
            _contextFactoryMock = new Mock<IContextFactory>(MockBehavior.Strict);
            _messageServiceMock = new Mock<IMessageService>(MockBehavior.Strict);
            _command = new CreateTenantEmploymentReferenceCommand
            {
                TenantId = new Guid("48D9AEA3-FDF6-46EE-A0D7-DFCC64D7FCEC"),
                FirstName = "Test1",
                LastName = "Test2",
                JobTitle = "Test3",
                Relationship = "Test4",
                Email = "Test5@test.test",
                PhoneNumber = new Totira.Bussiness.UserService.Commands.EmploymentReferencePhoneNumber { CountryCode = "002", Number = "new test number" },

            };
        }
        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new CreateTenantEmploymentReferenceCommandHandler(_tenantEmploymentReferenceRepositoryMock.Object,
                _loggerMock.Object,
                _contextFactoryMock.Object,
                _messageServiceMock.Object);
            //var contextMock =new Mock<IContext>();

            //Act
            await handler.HandleAsync(null, _command);

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
            var handler = new CreateTenantEmploymentReferenceCommandHandler(_tenantEmploymentReferenceRepositoryMock.Object,
                _loggerMock.Object,
                _contextFactoryMock.Object,
                _messageServiceMock.Object);

            //Act
            await handler.HandleAsync(null, _command);

            //Assert
            var employmentReferenceList = await _tenantEmploymentReferenceRepositoryMock.Object.Get(x => true);
            employmentReferenceList.Count().ShouldBe(2);
        }



    }



}

