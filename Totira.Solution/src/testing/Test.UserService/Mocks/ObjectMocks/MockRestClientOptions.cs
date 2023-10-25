using Microsoft.Extensions.Options;
using Moq;
using Totira.Support.Api.Options;

namespace Test.UserService.Mocks.ObjectMocks
{
    public static class MockRestClientOptions
    {
        public static Mock<IOptions<RestClientOptions>> GetIOptionsMock()
        {
            var restClientOptions = new Mock<IOptions<RestClientOptions>>();

            RestClientOptions optionsValue = new RestClientOptions();
            optionsValue.ThirdPartyIntegration = "http://totira.services.thirdpartyintegrationservice/api/v1";

            restClientOptions.Setup(o => o.Value).Returns(optionsValue);

            return restClientOptions;
        }
    }
}
