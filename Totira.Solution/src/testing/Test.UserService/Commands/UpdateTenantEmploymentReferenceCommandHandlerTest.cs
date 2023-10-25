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
    public class UpdateTenantEmploymentReferenceCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantEmploymentReference, Guid>> _tenantEmploymentReferenceRepositoryMock;
        private readonly Mock<ILogger<CreateTenantEmploymentReferenceCommandHandler>> _loggerMock;
        private readonly Mock<IContextFactory> _contextFactoryMock;
        private readonly Mock<IMessageService> _messageServiceMock;
        private UpdateTenantEmploymentReferenceCommand _command;
        public UpdateTenantEmploymentReferenceCommandHandlerTest()
        {
            _tenantEmploymentReferenceRepositoryMock = MockTenantEmploymentReferenceRepository.GetTenantTenantEmploymentReferenceRepository();
            _loggerMock = new Mock<ILogger<CreateTenantEmploymentReferenceCommandHandler>>();
            _contextFactoryMock = new Mock<IContextFactory>(MockBehavior.Strict);
            _messageServiceMock = new Mock<IMessageService>(MockBehavior.Strict);
        }
        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new UpdateTenantEmploymentReferenceCommandHandler(_tenantEmploymentReferenceRepositoryMock.Object,
                _loggerMock.Object,
                _contextFactoryMock.Object,
                _messageServiceMock.Object);
            _command = new UpdateTenantEmploymentReferenceCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                FirstName = "UpdateTest1",
                LastName = "UpdateTest2",
                JobTitle = "UpdateTest3",
                Relationship = "UpdateTest4",
                Email = "UpdateTest5@test.test",
                PhoneNumber = new Totira.Bussiness.UserService.Commands.EmploymentReferencePhoneNumber { CountryCode = "002", Number = "Update new test number" },

            };

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
        public async Task HandleAsyncTest_NonExistantId()
        {
            //Arrange
            var handler = new UpdateTenantEmploymentReferenceCommandHandler(_tenantEmploymentReferenceRepositoryMock.Object,
                _loggerMock.Object,
                _contextFactoryMock.Object,
                _messageServiceMock.Object);
            _command = new UpdateTenantEmploymentReferenceCommand
            {
                TenantId = Guid.NewGuid(),
                FirstName = "UpdateTest1",
                LastName = "UpdateTest2",
                JobTitle = "UpdateTest3",
                Relationship = "UpdateTest4",
                Email = "UpdateTest5@test.test",
                PhoneNumber = new Totira.Bussiness.UserService.Commands.EmploymentReferencePhoneNumber { CountryCode = "002", Number = "Update new test number" },
            };
            //Act
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => handler.HandleAsync(null, _command));

            //Assert
        }

    }
}

