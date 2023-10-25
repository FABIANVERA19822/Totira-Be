using RabbitMQ.Client;

namespace Totira.Support.EventServiceBus.RabittMQ
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }
        void TryConnect();
        IModel CreateModel();
    }
}
