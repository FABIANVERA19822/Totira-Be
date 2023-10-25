using LanguageExt;

namespace Totira.Support.Application.Messages
{
    public interface IMessageHandler
    {
        public interface IMessageHandler<TMessage> where TMessage : IMessage
        {
            Task HandleAsync(IContext context, Either<System.Exception, TMessage> message);
        }
    }
}
