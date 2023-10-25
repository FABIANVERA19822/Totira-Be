using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq.Expressions;
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
    public class AcceptTermsAndConditionsCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantTermsAndConditionsAcceptanceInfo, Guid>>
            _tenantTermsAndConditionsAcceptanceInfoMockRepository;
        private readonly Mock<ILogger<AcceptTermsAndConditionsCommandHandler>> _loggerMock;
        private readonly AcceptTermsAndConditionsCommand _command;
        private readonly Mock<IContextFactory> _contextFactoryMock;
        private readonly Mock<IMessageService> _messageServiceMock;
        public AcceptTermsAndConditionsCommandHandlerTest()
        {
            _tenantTermsAndConditionsAcceptanceInfoMockRepository = MockTenantTermsAndConditionsAcceptanceInfoRepository.GetTenantTermsAndConditionsAcceptanceInfoRepository();

            _command = new AcceptTermsAndConditionsCommand()
            {

                TenantId = new Guid("CF0A8C1C-F2D1-41A1-A12C-53D9BE513A1C"),
                SigningDateTime = DateTime.Now,
            };
            _loggerMock = new Mock<ILogger<AcceptTermsAndConditionsCommandHandler>>();

            _contextFactoryMock = MockIContextFactory.GetIContextFactoryMock();
            _messageServiceMock = MockIMessageService.GetIMessageServiceMock();
        }


        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new AcceptTermsAndConditionsCommandHandler(_tenantTermsAndConditionsAcceptanceInfoMockRepository.Object,
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
            Expression<Func<TenantTermsAndConditionsAcceptanceInfo, bool>> filter = f => f.TenantId == _command.TenantId;
            var tenantTermsAndConditionsAcceptanceInfos = await _tenantTermsAndConditionsAcceptanceInfoMockRepository
                .Object.Get(filter);

            tenantTermsAndConditionsAcceptanceInfos.Count().ShouldBe(1);
            tenantTermsAndConditionsAcceptanceInfos?.FirstOrDefault()?.TenantId.ShouldBeEquivalentTo(new Guid("CF0A8C1C-F2D1-41A1-A12C-53D9BE513A1C"));

        }


        [Fact]
        public async Task HandleAsyncTest_RepeatedId()
        {
            //Arrange
            var handler = new AcceptTermsAndConditionsCommandHandler(_tenantTermsAndConditionsAcceptanceInfoMockRepository.Object,
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
            var employmentReferenceList = await _tenantTermsAndConditionsAcceptanceInfoMockRepository.Object.Get(x => true);
            employmentReferenceList.Count().ShouldBe(2);
        }

    }
}
