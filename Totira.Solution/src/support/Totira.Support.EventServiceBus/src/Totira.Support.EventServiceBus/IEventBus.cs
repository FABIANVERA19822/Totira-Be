using Totira.Support.Application.Messages;

namespace Totira.Support.EventServiceBus
{
    public interface IEventBus
    {
        Task PublishAsync(IContext context, IMessage message);
        Task PublishAsync(string serializedMessageBody, string routingKey);
        Task SubscribeAsync<TMessage>() where TMessage : IMessage;
    }
}
