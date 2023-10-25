using Microsoft.Extensions.Logging;
using MongoDB.Driver;
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
    public class CreateTenantAcquaintanceReferralFormInfoCommandHandlerTest
    {
        private readonly Mock<IRepository<TenantAcquaintanceReferralFormInfo, Guid>> _tenantAcquaintanceReferralFormInfoRepositoryMock;
        private readonly Mock<IRepository<TenantAcquaintanceReferrals, Guid>> _tenantAcquaintanceReferralsRepositoryMock;
        private readonly Mock<ILogger<CreateTenantAcquaintanceReferralCommandHandler>> _loggerMock;
        private CreateAcquaintanceReferralFormInfoCommand _command;
        private Mock<IContextFactory> _contextFactoryMock;
        private Mock<IMessageService> _messageServiceMock;

        public CreateTenantAcquaintanceReferralFormInfoCommandHandlerTest()
        {
            _tenantAcquaintanceReferralFormInfoRepositoryMock = MockTenantAcquaintanceReferralFormInfoRepository.GetTenantAcquaintanceReferralFormInfoRepository();
            _tenantAcquaintanceReferralsRepositoryMock = MockTenantAcquaintanceReferralsRepository.GetTenantAcquaintanceReferralsRepository();
            _loggerMock = new Mock<ILogger<CreateTenantAcquaintanceReferralCommandHandler>>();
            _contextFactoryMock = MockIContextFactory.GetIContextFactoryMock();
            _messageServiceMock = MockIMessageService.GetIMessageServiceMock();

            _command = new CreateAcquaintanceReferralFormInfoCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                ReferralId = new Guid("{E471131F-60C0-46F6-A980-11A37BE97473}"),
                StarScore = 5,
                Feedback = "He is perfect man"
            };
        }

        [Fact]
        public async Task HandleAsyncTest_RequestOk_ReturnsOk()
        {
            //Arrange
            var handler = new CreateAcquaintanceReferralFormInfoCommandHandler(_loggerMock.Object,
                                                                               _tenantAcquaintanceReferralFormInfoRepositoryMock.Object,
                                                                               _tenantAcquaintanceReferralsRepositoryMock.Object,
                                                                               _contextFactoryMock.Object,
                                                                               _messageServiceMock.Object
                                                                              );
            var context = new Mock<IContext>();
            context.SetupGet(x => x.Href).Returns("");
            context.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
            context.SetupGet(x => x.CreatedBy).Returns(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"));
            context.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);

            //Act
            await handler.HandleAsync(context.Object, _command);

            //Assert
            var ReferralFeedback = await _tenantAcquaintanceReferralFormInfoRepositoryMock.Object.GetByIdAsync(_command.ReferralId);
            var tenantReferrals = await _tenantAcquaintanceReferralsRepositoryMock.Object.GetByIdAsync(_command.TenantId);

            ReferralFeedback.Score.ShouldBe(_command.StarScore);
            _tenantAcquaintanceReferralFormInfoRepositoryMock.Verify(ps => ps.Add(ReferralFeedback), Times.Once); 
            _tenantAcquaintanceReferralsRepositoryMock.Verify(ps=> ps.Update(tenantReferrals),Times.Once); 
        }


        [Fact]
        public async Task HandleAsyncTest_RequestFail_ThrowsException()
        {
            //Arrange
            var handler = new CreateAcquaintanceReferralFormInfoCommandHandler(_loggerMock.Object,
                                                                               _tenantAcquaintanceReferralFormInfoRepositoryMock.Object,
                                                                               _tenantAcquaintanceReferralsRepositoryMock.Object,
                                                                               null,
                                                                               null                                  
                                                                               );

            var _commandFail = new CreateAcquaintanceReferralFormInfoCommand
            {
                TenantId = new Guid(),
                ReferralId = new Guid(),
                StarScore = 0,
                Feedback = string.Empty
            };
            //Act
            await Assert.ThrowsAnyAsync<Exception>(async () => await handler.HandleAsync(null, _commandFail));

        }
    }
}
