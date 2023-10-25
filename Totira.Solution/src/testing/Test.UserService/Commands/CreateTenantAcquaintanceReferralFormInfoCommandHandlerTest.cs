using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Support.Application.Messages;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Commands
{
    public class CreateTenantAcquaintanceReferralFormInfoCommandHandlerTest : BaseCommandHandlerTest<CreateAcquaintanceReferralFormInfoCommand, CreateAcquaintanceReferralFormInfoCommandHandler>
    {
        private readonly Mock<IRepository<TenantAcquaintanceReferralFormInfo, Guid>> _tenantAcquaintanceReferralFormInfoRepositoryMock;
        private readonly Mock<IRepository<TenantAcquaintanceReferrals, Guid>> _tenantAcquaintanceReferralsRepositoryMock;

        protected override CreateAcquaintanceReferralFormInfoCommandHandler CommandHandler =>
            new CreateAcquaintanceReferralFormInfoCommandHandler(
                _loggerMock.Object,
                _tenantAcquaintanceReferralFormInfoRepositoryMock.Object,
                _tenantAcquaintanceReferralsRepositoryMock.Object,
                _contextFactoryMock.Object,
                _messageServiceMock.Object);

        public CreateTenantAcquaintanceReferralFormInfoCommandHandlerTest()
            : base(new CreateAcquaintanceReferralFormInfoCommand
            {
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                ReferralId = new Guid("{E471131F-60C0-46F6-A980-11A37BE97473}"),
                StarScore = 5,
                Feedback = "He is perfect man"
            })
        {
            _tenantAcquaintanceReferralFormInfoRepositoryMock = MockTenantAcquaintanceReferralFormInfoRepository.GetTenantAcquaintanceReferralFormInfoRepository();
            _tenantAcquaintanceReferralsRepositoryMock = MockTenantAcquaintanceReferralsRepository.GetTenantAcquaintanceReferralsRepository();
        }

        [Fact]
        public async Task HandleAsyncTest_RequestOk_ReturnsOk()
        {
            //Arrange
            var context = new Mock<IContext>();
            context.SetupGet(x => x.Href).Returns("");
            context.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
            context.SetupGet(x => x.CreatedBy).Returns(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"));
            context.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);

            //Act
            await CommandHandler.HandleAsync(context.Object, _command);

            //Assert
            var ReferralFeedback = await _tenantAcquaintanceReferralFormInfoRepositoryMock.Object.GetByIdAsync(_command.ReferralId);
            var tenantReferrals = await _tenantAcquaintanceReferralsRepositoryMock.Object.GetByIdAsync(_command.TenantId);

            ReferralFeedback.Score.ShouldBe(_command.StarScore);
            _tenantAcquaintanceReferralFormInfoRepositoryMock.Verify(ps => ps.Add(ReferralFeedback), Times.Once); 
            _tenantAcquaintanceReferralsRepositoryMock.Verify(ps=> ps.Update(tenantReferrals),Times.Once); 
        }


        [Fact]
        public async Task HandleAsyncTest_RequestFail_DoesntCreateNewEntity()
        {

            //Arrange 
            var _commandFail = new CreateAcquaintanceReferralFormInfoCommand
            {
                TenantId = new Guid(),
                ReferralId = new Guid(),
                StarScore = 0,
                Feedback = string.Empty
            };
              
            //Act
            await CommandHandler.HandleAsync(null, _commandFail);

            //Assert 

            var tenantAcquaintanceReferralFormInfo= await _tenantAcquaintanceReferralFormInfoRepositoryMock.Object.Get(x => true);
            tenantAcquaintanceReferralFormInfo.Count().ShouldBe(2);
             

        }
    }
}
