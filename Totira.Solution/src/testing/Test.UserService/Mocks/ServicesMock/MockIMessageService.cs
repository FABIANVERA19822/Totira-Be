using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;

namespace Test.UserService.Mocks.ServicesMock
{
    public static class MockIMessageService
    {
        public static Mock<IMessageService> GetIMessageServiceMock()
        {
            var serviceMock = new Mock<IMessageService>();

            serviceMock.Setup(s => s.SendAsync(It.IsAny<IContext>(), It.IsAny<IMessage>()))
                .ReturnsAsync(Guid.NewGuid());

            return serviceMock;
        }
    }
}
