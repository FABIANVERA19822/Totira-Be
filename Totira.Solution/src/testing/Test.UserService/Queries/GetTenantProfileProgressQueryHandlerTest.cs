using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Test.UserService.Mocks.ObjectMocks;
using Test.UserService.Mocks.RepoMocks;
using Test.UserService.Mocks.ResponseMocks;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Queries
{
    public class GetTenantProfileProgressQueryHandlerTest
    {
        private readonly Mock<IRepository<TenantApplicationDetails, Guid>> _applicationDetailsRepositoryMock;
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _basicInfoRepositoryMock;
        private readonly Mock<IRepository<TenantContactInformation, Guid>> _contactInfoRepositoryMock;
        private readonly Mock<IRepository<TenantEmployeeIncomes, Guid>> _employeeIncomesRepositoryMock;
        private Mock<IQueryRestClient> _queryRestClientMock;
        private readonly Mock<IOptions<RestClientOptions>> _restOptionsMock;

        public GetTenantProfileProgressQueryHandlerTest()
        {
            _applicationDetailsRepositoryMock = MockTenantApplicationDetailsRepository.GetTenantApplicationDetailsRepository();
            _basicInfoRepositoryMock = MockTenantBasicInformationRepository.GetTenantBasicInformationRepository();
            _contactInfoRepositoryMock = MockTenantContactInformationRepository.GetTenantContactInformationRepository();
            _employeeIncomesRepositoryMock = MockTenantEmployeeIncomesRepository.GetTenantEmployeeIncomesRepository();
            _restOptionsMock = MockRestClientOptions.GetIOptionsMock();
            _queryRestClientMock = new Mock<IQueryRestClient>();
        }
        [Fact]
        public async Task HandleAsyncTest_Ok()
        {
            //Arrange
            _queryRestClientMock = MockQueryRestClient_GetPersonaApplicationDto.GetMockAcceptedIQueryRestClient();

            var handler = new GetTenantProfileProgressQueryHandler(_applicationDetailsRepositoryMock.Object,
                                                                   _basicInfoRepositoryMock.Object,
                                                                   _contactInfoRepositoryMock.Object,
                                                                   _employeeIncomesRepositoryMock.Object,
                                                                   _queryRestClientMock.Object,
                                                                   _restOptionsMock.Object);

            var query = new QueryTenantProfileProgressByTenantId(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<Dictionary<string, int>>();
            result.Sum(x => x.Value).ShouldBe(100);
        }

        [Fact]
        public async Task HandleAsyncTest_Ok_ShouldBeCero()
        {
            //Arrange
            _queryRestClientMock = MockQueryRestClient_GetPersonaApplicationDto.GetMockRejectedIQueryRestClient();
            var handler = new GetTenantProfileProgressQueryHandler(_applicationDetailsRepositoryMock.Object,
                                                                   _basicInfoRepositoryMock.Object,
                                                                   _contactInfoRepositoryMock.Object,
                                                                   _employeeIncomesRepositoryMock.Object,
                                                                   _queryRestClientMock.Object,
                                                                   _restOptionsMock.Object);

            var query = new QueryTenantProfileProgressByTenantId(Guid.NewGuid()); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<Dictionary<string, int>>();
            result.Sum(x => x.Value).ShouldBe(0);
        }
    }
}
