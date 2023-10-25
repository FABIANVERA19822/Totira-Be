namespace Test.ThirdPartyIntegrationService.Mocks.BusMock
{
    using Moq;
    using Totira.Support.Application.Messages;
    using Totira.Support.EventServiceBus;

    public static class MockIEventBus
    {
        public static Mock<IEventBus> GetIEventBusMock()
        {
            var serviceMock = new Mock<IEventBus>();
            var task = new Mock<Task>();
            serviceMock.Setup(s => s.PublishAsync(It.IsAny<IContext>(), It.IsAny<IMessage>()));

            return serviceMock;
        }
    }
}