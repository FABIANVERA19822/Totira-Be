using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Api.Options;
using Totira.Support.CommonLibrary.Settings;

namespace Test.UserService.Mocks.ObjectMocks
{
    public static class MockIOptionFrontendSettings
    {
        public static Mock<IOptions<FrontendSettings>> GetMockedIOptionFrontendSettings()
        {
            #region Mocking data
            FrontendSettings mockedFrontend = new FrontendSettings();
            mockedFrontend.Url = "testURL";
            #endregion

            #region Instanciate the Mock
            var restClientOptions = new Mock<IOptions<FrontendSettings>>();
            #endregion

            #region Set up
            restClientOptions.Setup(o => o.Value).Returns(mockedFrontend);
            #endregion

            return restClientOptions;
        }
    }
}
