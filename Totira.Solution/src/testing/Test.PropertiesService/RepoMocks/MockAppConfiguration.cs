using Moq;
using System.Net;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Bussiness.PropertiesService.Configuration;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;

namespace Test.PropertiesService.RepoMocks
{
    public class MockAppConfiguration
    {
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

        public static Mock<IQueryRestClient> GetQueryRestClient()
        {
            #region MockingData


            QueryResponse<GetPropertyLongitudeAndLatitudeDto> queryResponse = new QueryResponse<GetPropertyLongitudeAndLatitudeDto>
                (
                    HttpStatusCode.OK,
                    new GetPropertyLongitudeAndLatitudeDto(43.6485693, -79.3649204)
                );

            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IQueryRestClient>();
            #endregion

            #region SetupAllMethods

            mockRepo.Setup(r => r.GetAsync<GetPropertyLongitudeAndLatitudeDto>(It.IsAny<string>()))
                    .ReturnsAsync(queryResponse);


            #endregion
            return mockRepo;

        }
    }
}
