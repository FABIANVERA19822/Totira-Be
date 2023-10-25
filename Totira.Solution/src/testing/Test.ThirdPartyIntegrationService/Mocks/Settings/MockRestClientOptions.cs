

namespace Test.ThirdPartyIntegrationService.Mocks.Settings
{
    using Microsoft.Extensions.Options;
    using Moq;
    using Totira.Support.Api.Options;
    public static class MockRestClientOptions
    {
        public static Mock<IOptions<RestClientOptions>> GetIOptionsMock()
        {
            var restClientOptions = new Mock<IOptions<RestClientOptions>>();

            RestClientOptions optionsValue = new RestClientOptions();
            optionsValue.ThirdPartyIntegration = "http://totira.services.thirdpartyintegrationservice/api/v1";
            optionsValue.User = "http://totira.services.userservice/api/v1";
            optionsValue.Tenant = "http://totira.services.tenantservice/api/v1";
            optionsValue.Properties = "http://totira.services.propertiesservice/api/v1";

            restClientOptions.Setup(o => o.Value).Returns(optionsValue);

            return restClientOptions;
        }
    }
}


