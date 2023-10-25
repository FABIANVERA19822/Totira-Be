using Totira.Support.Application.Messages;

namespace Totira.Support.EventServiceBus
{
    public interface IEventBus
    {
        Task PublishAsync(IContext context, IMessage message);
        Task PublishAsync(string serializedMessageBody, string routingKey);
        Task SubscribeAsync<TMessage>() where TMessage : IMessage;
    }

    public interface IEventBusServiceable : IEventBus
    {
        Task StartAsync(CancellationToken cancellationToken);
    }

    public interface IEventPublisher
    {
        Task PublishAsync(string serializedMessageBody, string routingKey);

        Task PublishAsync(IContext context, IMessage message);
    }

    public interface IEventSubscriber
    {
        Task SubscribeAsync(CancellationToken cancellationToken);
    }
}
