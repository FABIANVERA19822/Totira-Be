using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Moq;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Queries;
using Shouldly;
using Totira.Business.ThirdPartyIntegrationService.Domain.Persona;
using Microsoft.Extensions.Options;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Bussiness.UserService.Common;
using Test.UserService.Mocks.RepoMocks;
using Test.UserService.Mocks.ObjectMocks;
using Test.UserService.Mocks.ResponseMocks;
using Test.UserService.Mocks.CommonMocks;

namespace Test.UserService.Queries
{
    public class GetTenantProfileFunnelByTenantIdQueryHandlerTest
    {
        private readonly Mock<IRepository<TenantApplicationDetails, Guid>> _applicationDetailsRepositoryMock;
        private readonly Mock<IRepository<TenantBasicInformation, Guid>> _basicInfoRepositoryMock;
        private readonly Mock<IRepository<TenantContactInformation, Guid>> _contactInfoRepositoryMock;
        private readonly Mock<IRepository<TenantEmployeeIncomes, Guid>> _employeeIncomesRepositoryMock;
        private readonly Mock<IRepository<TenantRentalHistories, Guid>> _tenantRentalHistoriesRepositoryMock;
        private readonly Mock<IRepository<TenantAcquaintanceReferrals, Guid>> _tenantAcquaintanceReferralsRepositoryMock;
        private readonly Mock<IRepository<TenantApplicationType, Guid>> _tenantApplicationTypeRepositoryMock;
        private readonly Mock<ICommonFunctions> _commonFunctions;
        private Mock<IQueryRestClient> _queryRestClientMock;
        private readonly Mock<IOptions<RestClientOptions>> _restOptionsMock;

        public GetTenantProfileFunnelByTenantIdQueryHandlerTest()
        {
            _applicationDetailsRepositoryMock = MockTenantApplicationDetailsRepository.GetTenantApplicationDetailsRepository();
            _basicInfoRepositoryMock = MockTenantBasicInformationRepository.GetTenantBasicInformationRepository();
            _contactInfoRepositoryMock = MockTenantContactInformationRepository.GetTenantContactInformationRepository();
            _employeeIncomesRepositoryMock = MockTenantEmployeeIncomesRepository.GetTenantEmployeeIncomesRepository();
            _tenantRentalHistoriesRepositoryMock = MockTenantRentalHistoriesRepository.GetTenantRentalHistoriesRepository();
            _tenantAcquaintanceReferralsRepositoryMock = MockTenantAcquaintanceReferralsRepository.GetTenantAcquaintanceReferralsRepository();
            _tenantApplicationTypeRepositoryMock = new Mock<IRepository<TenantApplicationType, Guid>>();
            _commonFunctions = MockCommonFunction.GetCommonFunctionsMock();
            _restOptionsMock = MockRestClientOptions.GetIOptionsMock();
        }

        [Fact]
        public async Task HandleAsyncTest_Approved()
        {
            //Arrange
            _queryRestClientMock = MockQueryRestClient_GetPersonaApplicationDto.GetMockAcceptedIQueryRestClient();

            var handler = new GetTenantProfileFunnelByTenantIdQueryHandler(_applicationDetailsRepositoryMock.Object,
                                                                           _basicInfoRepositoryMock.Object,
                                                                           _contactInfoRepositoryMock.Object,
                                                                           _employeeIncomesRepositoryMock.Object,
                                                                           _tenantRentalHistoriesRepositoryMock.Object,
                                                                           _tenantAcquaintanceReferralsRepositoryMock.Object,
                                                                           _tenantApplicationTypeRepositoryMock.Object,
                                                                           _commonFunctions.Object,
                                                                           _queryRestClientMock.Object,
                                                                           _restOptionsMock.Object);

            var query = new QueryTenantProfileFunnelByTenantId(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<Dictionary<string, bool>>();
        }

        [Fact]
        public async Task HandleAsyncTest_Rejected()
        {
            //Arrange
            _queryRestClientMock = MockQueryRestClient_GetPersonaApplicationDto.GetMockRejectedIQueryRestClient();

            var handler = new GetTenantProfileFunnelByTenantIdQueryHandler(_applicationDetailsRepositoryMock.Object,
                                                                           _basicInfoRepositoryMock.Object,
                                                                           _contactInfoRepositoryMock.Object,
                                                                           _employeeIncomesRepositoryMock.Object,
                                                                           _tenantRentalHistoriesRepositoryMock.Object,
                                                                           _tenantAcquaintanceReferralsRepositoryMock.Object,
                                                                           _tenantApplicationTypeRepositoryMock.Object,
                                                                           _commonFunctions.Object,
                                                                           _queryRestClientMock.Object,
                                                                           _restOptionsMock.Object);

            var query = new QueryTenantProfileFunnelByTenantId(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513333")); // should be mocked data id

            //Act
            var result = await handler.HandleAsync(query);

            //Assert
            result.ShouldBeOfType<Dictionary<string, bool>>();
        }
    }
}
