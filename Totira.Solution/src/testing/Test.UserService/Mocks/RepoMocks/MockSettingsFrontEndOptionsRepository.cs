namespace Test.UserService.RepoMocks
{
    using Microsoft.Extensions.Options;
    using Moq;
    using Totira.Support.CommonLibrary.Settings;
    public static class MockSettingsFrontEndOptionsRepository
    {
        public static Mock<IOptions<FrontendSettings>> GetSettingsRepository()
        {
            #region MockingData
            var settings = new FrontendSettings() { Url= "http://localhost:62112/" };
            #endregion

            #region instanciateTheMock
            var mockRepo = new Mock<IOptions<FrontendSettings>>();
            #endregion

            #region SetupAllMethods


            mockRepo.Setup(r => r.Value).Returns(settings);
            #endregion
            return mockRepo;
        }
    }
}
