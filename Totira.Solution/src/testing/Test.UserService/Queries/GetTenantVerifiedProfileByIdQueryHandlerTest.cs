using Moq;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Queries;
using Totira.Bussiness.UserService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using Totira.Business.ThirdPartyIntegrationService.Domain.Persona;
using Totira.Support.Api.Connection;
using Totira.Bussiness.UserService.Configuration;
using Test.UserService.Mocks.RepoMocks;

namespace Test.UserService.Queries
{
    public class GetTenantVerifiedProfileByIdQueryHandlerTest
    {

        private readonly Mock<IRepository<TenantPersonaValidation, string>> _tenantPersonalValidationRepositoryMock;
        private readonly Mock<IRepository<TenantEmployeeInconmeValidation, string>> _tenantJiraValidationRepositoryMock;

        private readonly Mock<IQueryRestClient> _queryRestClientMock;
        private readonly Mock<IAppConfiguration> _appConfigurationMock;

        public GetTenantVerifiedProfileByIdQueryHandlerTest()
        {

            _tenantPersonalValidationRepositoryMock = MockTenantVerifiedbyProfile.GetPersonaRepository();
            _tenantJiraValidationRepositoryMock = MockTenantVerifiedbyProfile.GetJiraValidationRepository();
            _queryRestClientMock = new Mock<IQueryRestClient>();
            _appConfigurationMock = MockTenantVerifiedbyProfile.GetAppConfiguration();
        }
 

        [Fact]
        public async Task HandleAsync_TenantIdDoesntHaveVerifications_VerificationIsEmptyOk()
        {
            //Arrange


            Guid tenantId =Guid.NewGuid();
                
            var query = new QueryTenantVerifiedProfileById(tenantId, new GetTenantEmployeeIncomesDto(tenantId));
       
            //Act

            //Assert

        }

        [Fact]
        public async Task HandleAsync_TenantIdHasVerifications_Ok()
        {
            //Arrange

             
            var query = MockTenantVerifiedbyProfile.GetQueryTenantVerifiedProfile_Ok();

            //Act

            //Assert

        }
    }
}
