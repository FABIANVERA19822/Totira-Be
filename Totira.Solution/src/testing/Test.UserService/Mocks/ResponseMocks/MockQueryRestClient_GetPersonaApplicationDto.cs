using Moq;
using System.Net;
using Totira.Bussiness.UserService.DTO.ThirdpartyService;
using Totira.Support.Api.Connection;

namespace Test.UserService.Mocks.ResponseMocks
{
    public static class MockQueryRestClient_GetPersonaApplicationDto
    {
        public static Mock<IQueryRestClient> GetMockAcceptedIQueryRestClient()
        {
            # region mockData
            var personaApplicationAccepted = new GetPersonaApplicationDto
            {
                Status = "approved",
                InquiryId = "",
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"),
                UrlImages = new List<string>()
            };
            var personaResponseAccepted = new QueryResponse<GetPersonaApplicationDto>(HttpStatusCode.OK, personaApplicationAccepted);
            #endregion

            #region instanciateTheMock
            var mockQueryRestClient = new Mock<IQueryRestClient>();
            #endregion

            #region setup
            mockQueryRestClient.Setup(o => o.GetAsync<GetPersonaApplicationDto>(It.IsAny<string>()))
                .Returns((string url) => Task.FromResult(personaResponseAccepted));
            #endregion

            return mockQueryRestClient;
        }

        public static Mock<IQueryRestClient> GetMockRejectedIQueryRestClient()
        {
            # region mockData
            var personaApplicationRejected = new GetPersonaApplicationDto
            {
                Status = "rejected",
                InquiryId = "",
                TenantId = new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513333"),
                UrlImages = new List<string>()
            };

            var personaResponseRejected = new QueryResponse<GetPersonaApplicationDto>(HttpStatusCode.Accepted, personaApplicationRejected);
            #endregion

            #region instanciateTheMock
            var mockQueryRestClient = new Mock<IQueryRestClient>();
            #endregion

            #region setup
            mockQueryRestClient.Setup(o => o.GetAsync<GetPersonaApplicationDto>(It.IsAny<string>()))
                .Returns((string tenantId) => Task.FromResult(personaResponseRejected));
            #endregion

            return mockQueryRestClient;
        }
    }
}
