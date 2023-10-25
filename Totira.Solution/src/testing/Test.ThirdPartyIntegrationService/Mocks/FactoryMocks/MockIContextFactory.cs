using Moq;
using Totira.Support.Application.Messages;

namespace Test.ThirdPartyIntegrationService.Mocks.FactoryMocks
{
    public static class MockIContextFactory
    {
        public static Mock<IContextFactory> GetIContextFactoryMock()
        {
            var mock =  new Mock<IContextFactory>();

            var mockedContext = new Mock<IContext>();
            mockedContext.SetupGet(x => x.Href).Returns("");
            mockedContext.SetupGet(x => x.TransactionId).Returns(Guid.NewGuid);
            mockedContext.SetupGet(x => x.CreatedBy).Returns(new Guid("CF0A8C1C-F2D0-41A1-A12C-53D9BE513A1C"));
            mockedContext.SetupGet(x => x.CreatedOn).Returns(DateTimeOffset.Now);

            mock.Setup(c=>c.Create(It.IsAny<string>(),It.IsAny<Guid>()))
                .Returns(mockedContext.Object);

            return mock;
        }
    }
}
