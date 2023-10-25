namespace Test.ThirdPartyIntegrationService.Mocks.Settings
{
    using Microsoft.Extensions.Options;
    using Moq;
    using Totira.Business.ThirdPartyIntegrationService.Options;

    public static class MockIOptionsJiraOptions
    {
        public static Mock<IOptions<JiraOptions>> GetIOptionsJiraMock()
        {
            var serviceMock = new Mock<IOptions<JiraOptions>>();

            var jiraOptions = new JiraOptions
            {
                StatusComplete = "Done",
                StatusRejected = "Rejected"
            };

            serviceMock.Setup(s => s.Value).Returns(jiraOptions);
            return serviceMock;
        }
    }
}