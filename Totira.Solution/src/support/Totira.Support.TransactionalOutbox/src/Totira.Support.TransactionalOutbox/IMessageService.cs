using Totira.Support.Application.Messages;

namespace Totira.Support.TransactionalOutbox
{
    public interface IMessageService
    {
        Task<Guid> SendAsync(IContext context, IMessage message);
        Task ProcessAsync(Guid messageId);
        Task ProcessPendingAsync(int count);
    }
}
