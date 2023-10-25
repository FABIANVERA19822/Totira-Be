using Totira.Support.Application.Messages;

namespace Totira.Support.Application.Dispatchers.Behaviours
{
    public delegate Task BehaviourHandlerDelegate<TMessage>(IContext context, TMessage message);
    public interface IBehaviour
    {
        Task HandleAsync<TMessage>(IContext context, TMessage message, BehaviourHandlerDelegate<TMessage> next)
            where TMessage : IMessage;
    }
}
