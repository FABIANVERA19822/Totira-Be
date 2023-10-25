
using Moq;
using Totira.Business.ThirdPartyIntegrationService.Domain.Certn;
using static Totira.Support.Persistance.IRepository;

namespace Test.ThirdPartyIntegrationService.RepoMocks.Certn
{
    public class MockCertnRepository
    {
        public static Mock<IRepository<TenantApplications, string>> GetTenantAppRepository()
        {
            #region MockingData
            var tenantApplications = new List<TenantApplication>
            {
                new TenantApplication
                {
                  Id="3fa85f64-5717-4562-b3fc-2c963f66afa6",
                  ApplicantId= Guid.Parse("642b4d18-30ff-4364-a5f6-1081ada8d482"),
                  StatusEquifax="NONE",
                  StatusSoftCheck="NONE",
                  Response="",
                  CreatedOn = DateTimeOffset.Now
                }
            };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IRepository<TenantApplications, string>>();
            #endregion
            return mockRepo;
        }
    }
}
