using Moq;
using static Totira.Support.Persistance.IRepository;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using System.Net;
using Totira.Support.Api.Connection;
using Totira.Bussiness.UserService.DTO.PropertyService;

namespace Test.UserService.Mocks.ResponseMocks
{
    public class MockQueryRestClient_GetPropertyDetailsDto
    {
        public static Mock<IQueryRestClient> GetMockAcceptedIQueryRestClient()
        {
            #region mockData

            var propertyExists = new GetPropertyDetailsDto()
            {
                Id = "C523423",
                Area ="134",
                Bedrooms =4,
                Washrooms=2
                
            };
            var propertyExistsResponse = new QueryResponse<GetPropertyDetailsDto>(HttpStatusCode.Accepted, propertyExists);
            #endregion

            #region instanciateTheMock
            var mockQueryRestClient = new Mock<IQueryRestClient>();
            #endregion

            #region setup
            mockQueryRestClient.Setup(o => o.GetAsync<GetPropertyDetailsDto>(It.IsAny<string>()))
                .Returns((string url) => Task.FromResult(propertyExistsResponse));
            #endregion

            return mockQueryRestClient;
        }
    }

    public class MockQueryRestClient_GetPropertyDetailstoApplyDto
    {
        public static Mock<IQueryRestClient> GetMockAcceptedIQueryRestClient()
        {
            #region mockData

            var propertyExists = new GetPropertyDetailstoApplyDto()
            {
                Id = "C5799045",
                Area = "134",
                Bedrooms = 4,
                Washrooms = 2

            };
            var propertyExistsResponse = new QueryResponse<GetPropertyDetailstoApplyDto>(HttpStatusCode.Accepted, propertyExists);
            #endregion

            #region instanciateTheMock
            var mockQueryRestClient = new Mock<IQueryRestClient>();
            #endregion

            #region setup
            mockQueryRestClient.Setup(o => o.GetAsync<GetPropertyDetailstoApplyDto>(It.IsAny<string>()))
                .Returns((string url) => Task.FromResult(propertyExistsResponse));
            #endregion

            return mockQueryRestClient;
        }
    }
}
