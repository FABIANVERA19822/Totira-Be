using Totira.Support.Application.Messages;

namespace Totira.Support.Application.Dispatchers
{
    public interface IDispatcher
    {
        Task SendAsync<TMessage>(IContext context, TMessage message) where TMessage : IMessage;

        Task SendAsync(Type messageType, IContext context, IMessage message);
    }
}
