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
    public class GetTenantRentalHistoriesByIdQueryHandlerTest
    {
        private readonly Mock<IRepository<TenantRentalHistories, Guid>> _tenantRentalHistoriesRepositoryMock;

        public GetTenantRentalHistoriesByIdQueryHandlerTest()
        {
            _tenantRentalHistoriesRepositoryMock = MockTenantRentalHistoriesRepository.GetTenantRentalHistoriesRepository();
        }

        [Fact]
        public async Task HandleAsyncTest_OK()
        {
            //Arrange
            var handler = new GetTenantRentalHistoriesByIdQueryHandler(_tenantRentalHistoriesRepositoryMock.Object);
            var query = new QueryTenantRentalHistoriesById(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantRentalHistoriesDto>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task HandleAsyncTest_BadId()
        {
            //Arrange
            var handler = new GetTenantRentalHistoriesByIdQueryHandler(_tenantRentalHistoriesRepositoryMock.Object);
            var query = new QueryTenantRentalHistoriesById(Guid.NewGuid());

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<GetTenantRentalHistoriesDto>();
            result.RentalHistories.ShouldBeNull();
        }
    }
}
