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
    public class GetTenantAcquaintanceReferralByIdQueryHandlerTest
    {
        private readonly Mock<IRepository<TenantAcquaintanceReferrals, Guid>> _tenantAcquaintanceReferralsRepositoryMock;

        public GetTenantAcquaintanceReferralByIdQueryHandlerTest()
        {
            _tenantAcquaintanceReferralsRepositoryMock = MockTenantAcquaintanceReferralsRepository.GetTenantAcquaintanceReferralsRepository();
        }
        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            var handler = new GetTenantAcquaintanceReferralByIdQueryHandler(_tenantAcquaintanceReferralsRepositoryMock.Object);
            var query = new QueryTenantAcquaintanceReferralById(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantAquaintanceReferralDto>();
            result.AquaintanceReferrals.ShouldNotBeNull();
            result.AquaintanceReferrals.Count.ShouldBeGreaterThan(-1);
        }

        [Fact]
        public async Task HandleAsyncTest_BadId()
        {
            //Arrange
            var handler = new GetTenantAcquaintanceReferralByIdQueryHandler(_tenantAcquaintanceReferralsRepositoryMock.Object);
            var query = new QueryTenantAcquaintanceReferralById(Guid.NewGuid());

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantAquaintanceReferralDto>();
            result.AquaintanceReferrals.ShouldBeNull();
        }
    }
}
