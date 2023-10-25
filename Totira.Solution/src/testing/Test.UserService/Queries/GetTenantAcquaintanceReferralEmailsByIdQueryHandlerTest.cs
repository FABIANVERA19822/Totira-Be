using Moq;
using Shouldly;
using Test.UserService.Mocks.RepoMocks;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries
{
    public class GetTenantAcquaintanceReferralEmailsByIdQueryHandlerTest
    {

        private readonly Mock<IRepository<TenantAcquaintanceReferrals, Guid>> _tenantAcquaintanceReferralsRepositoryMock;
        public GetTenantAcquaintanceReferralEmailsByIdQueryHandlerTest()
        {
            _tenantAcquaintanceReferralsRepositoryMock = MockTenantAcquaintanceReferralsRepository.GetTenantAcquaintanceReferralsRepository();
        }

        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            var handler = new GetTenantAcquaintanceReferralEmailsByIdQueryHandler(_tenantAcquaintanceReferralsRepositoryMock.Object);
            var query = new QueryTenantAcquaintanceReferralEmailsById(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantAquaintanceReferralEmailsDto>();
            result.AquaintanceReferralEmails.ShouldNotBeNull();
        }
        [Fact]
        public async Task HandleAsyncTest_BadId()
        {
            //Arrange
            var handler = new GetTenantAcquaintanceReferralEmailsByIdQueryHandler(_tenantAcquaintanceReferralsRepositoryMock.Object);
            var query = new QueryTenantAcquaintanceReferralEmailsById(Guid.NewGuid());

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantAquaintanceReferralEmailsDto>();
            result.AquaintanceReferralEmails.Count.ShouldBe(0);
        }
    }
}
