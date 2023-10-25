using Moq;
using Totira.Business.ThirdPartyIntegrationService.Domain.Jira;
using Totira.Business.ThirdPartyIntegrationService.Domain.Persona;
using Totira.Bussiness.UserService.Configuration;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Options;
using static Totira.Support.Persistance.IRepository;

namespace Test.UserService.Mocks.RepoMocks
{
    public class MockTenantVerifiedbyProfile
    {
        public static Mock<IRepository<TenantPersonaValidation, string>> GetPersonaRepository()
        {
            #region MockingData
            var tenantPersonaValidation = new List<TenantPersonaValidation>()
            {
                new TenantPersonaValidation()
                {
                    Status="RETURNED"
                }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantPersonaValidation, string>>();
            #endregion

            #region SetupAllMethods


            mockRepo.Setup(r => r.Get(x => true))
                  .ReturnsAsync(tenantPersonaValidation);

            #endregion


            return mockRepo;
        }
        public static Mock<IRepository<TenantEmployeeInconmeValidation, string>> GetJiraValidationRepository()
        {
            #region MockingData
            var IncomeValidation = new List<TenantEmployeeInconmeValidation>()
            {
                new TenantEmployeeInconmeValidation()
                {
                    Status="Pending"
                }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantEmployeeInconmeValidation, string>>();
            #endregion

            #region SetupAllMethods


            mockRepo.Setup(r => r.Get(x => true))
                  .ReturnsAsync(IncomeValidation);

            #endregion


            return mockRepo;
        }
        public static Mock<IAppConfiguration> GetAppConfiguration()
        {
            #region MockingData
            RestClientOptions restClientOptions = new RestClientOptions()
            { User = "", Properties = "", Tenant = "", ThirdPartyIntegration = "http://totira.services.thirdpartyintegrationservice/api/v1" };

            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IAppConfiguration>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.RestClient())
                    .Returns(restClientOptions);


            #endregion
            return mockRepo;

        }

        public static QueryTenantVerifiedProfileById GetQueryTenantVerifiedProfile_Ok()
        {
            Guid tenantId = Guid.NewGuid();
            var incomesDto = new Mock<GetTenantEmployeeIncomesDto>(tenantId);

            return new QueryTenantVerifiedProfileById(tenantId, incomesDto.Object);
        }
    }
}
