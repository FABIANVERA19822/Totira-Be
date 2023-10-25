namespace Totira.Support.Application.Messages
{
    public interface IMessageService
    {
        Task<Guid> SendAsync(IContext context, IMessage message);
        Task ProcessAsync(Guid messageId);
        Task ProcessPendingAsync(int count);
    }
}
