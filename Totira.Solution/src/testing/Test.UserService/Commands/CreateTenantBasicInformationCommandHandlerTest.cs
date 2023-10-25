using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Test.UserService.Mocks.FactoryMocks;
using Test.UserService.Mocks.RepoMocks;
using Test.UserService.Mocks.ServicesMock;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantBasicInformationCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _tenantPersonalInformationRepositoryMock;
        private readonly Mock<ILogger<CreateTenantBasicInformationCommandHandler>> _loggerMock;
        private readonly CreateTenantBasicInformationCommand _command;
        private Mock<IContextFactory> _contextFactoryMock;
        private Mock<IMessageService> _messageServiceMock;
        public CreateTenantBasicInformationCommandHandlerTest()
        {
            _tenantPersonalInformationRepositoryMock = MockTenantPersonalInformationRepository.GetTenantPersonalInformationRepository();
            _loggerMock = new Mock<ILogger<CreateTenantBasicInformationCommandHandler>>();
            _contextFactoryMock = MockIContextFactory.GetIContextFactoryMock();
            _messageServiceMock = MockIMessageService.GetIMessageServiceMock();
            _command = new CreateTenantBasicInformationCommand
            {
                Id = Guid.NewGuid(),
                FirstName = "Testy",
                LastName = "MacTest The Third",
                Birthday = new BasicInformationBirthday { Year = 1993, Day = 27, Month = 03 },
                AboutMe = "i'm the third me"
            };
        }

        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new CreateTenantBasicInformationCommandHandler(_tenantPersonalInformationRepositoryMock.Object, 
                                                                         _loggerMock.Object,
                                                                         _contextFactoryMock.Object,
                                                                         _messageServiceMock.Object);
            var context = new Mock<IContext>();
            context.SetupGet(x => x.Href).Returns("");
            context.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
            context.SetupGet(x => x.CreatedBy).Returns(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"));
            context.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);
            //Act
            await handler.HandleAsync(context.Object, _command);

            //Assert
            var personalInfo = await _tenantPersonalInformationRepositoryMock.Object.GetByIdAsync(_command.Id);
            personalInfo.FirstName.ShouldBe(_command.FirstName);
            personalInfo.LastName.ShouldBe(_command.LastName);
        }

        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            var handler = new CreateTenantBasicInformationCommandHandler(_tenantPersonalInformationRepositoryMock.Object,
                                                                         _loggerMock.Object,
                                                                         _contextFactoryMock.Object,
                                                                         _messageServiceMock.Object);
            var context = new Mock<IContext>();
            context.SetupGet(x => x.Href).Returns("");
            context.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
            context.SetupGet(x => x.CreatedBy).Returns(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"));
            context.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);
            //Act
            await handler.HandleAsync(context.Object, _command);

            //Assert
            //var referrals = await _tenantPersonalInformationRepositoryMock.Object.Get(x => true);
            //referrals.Count().ShouldBe(1);
        }
    }
}
