using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using Totira.Support.Application.Dispatchers;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.EventServiceBus.Exceptions;

namespace Totira.Support.EventServiceBus.RabittMQ
{
    public class RabbitMQEventBus : IEventBus
    {
        private const string EXCHANGE_NAME = "Totira.Services";
        private const string DLX_EXCHANGE_NAME = "DLX-Torira.Services";
        private const string DLX_QUEUE_NAME = "FailedMessages";
        private const int DELIVERY_MODE_PERSISTENT = 2;

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly IDispatcher _dispatcher;
        private readonly IModel _consumerChannel;
        private readonly string _queueName;
        private readonly Dictionary<string, Type> _handlers;

        public RabbitMQEventBus(
            IRabbitMQPersistentConnection persistentConnection,
            ILogger<RabbitMQEventBus> logger,
            IDispatcher dispatcher,
            string queueName = null)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _dispatcher = dispatcher;
            _queueName = queueName;
            _consumerChannel = CreateConsumerChannel();
            _handlers = new Dictionary<string, Type>();
        }

        public async Task PublishAsync(IContext context, IMessage message)
        {
            var routingKey = CustomAttributeExtensions.GetRoutingKey(message.GetType());

            if (routingKey == null)
                throw new MissingRoutingKeyException(message.GetType());

            var serializedEnvelope = JsonConvert.SerializeObject(new Envelope<IMessage>(context, message));
            await this.PublishAsync(serializedEnvelope, routingKey, message.GetType().Name);
        }

        public async Task PublishAsync(string serializedMessageBody, string routingKey)
        {
            if (string.IsNullOrEmpty(routingKey))
                throw new MissingRoutingKeyException();

            await this.PublishAsync(serializedMessageBody, routingKey, routingKey);
        }


        private async Task PublishAsync(string serializedMessageBody, string routingKey, string typeName = "")
        {
            await Task.Yield();

            try
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using (var channel = _persistentConnection.CreateModel())
                {
                    channel.ConfirmSelect();

                    var body = Encoding.UTF8.GetBytes(serializedMessageBody);

                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = DELIVERY_MODE_PERSISTENT;

                    channel.BasicPublish(EXCHANGE_NAME, routingKey, true, properties, body);
                    channel.WaitForConfirmsOrDie();
                }

                _logger.LogDebug($"Message of type {typeName} with routing key [{routingKey}] published");
            }
            catch (OperationInterruptedException ex)
            {
                _logger.LogError($"Could not publish message of type {typeName} with routing key [{routingKey}]");

                throw new OperationFailedException("Could not publish message", ex);
            }
            catch (BrokerUnreachableException ex)
            {
                throw new ConnectionFailedException("Could not connect to RabbitMQ", ex);
            }
            catch (ConnectFailureException ex)
            {
                throw new ConnectionFailedException("Could not connect to RabbitMQ", ex);
            }
        }

        public async Task SubscribeAsync<TMessage>() where TMessage : IMessage
        {
            await Task.Yield();

            var routingKey = CustomAttributeExtensions.GetRoutingKey<TMessage>();

            if (routingKey == null)
                throw new MissingRoutingKeyException(typeof(TMessage));

            _handlers.Add(routingKey, typeof(TMessage));

            try
            {
                _consumerChannel.QueueBind(_queueName, EXCHANGE_NAME, routingKey);

                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(_queueName, false, consumer);

                _logger.LogDebug($"Subscribed to receive {typeof(TMessage)} with routing key [{routingKey}]");
            }
            catch (ConnectFailureException ex)
            {
                throw new ConnectionFailedException("Could not connect to RabbitMQ", ex);
            }
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            try
            {
                var channel = _persistentConnection.CreateModel();

                channel.ExchangeDeclare(DLX_EXCHANGE_NAME, ExchangeType.Fanout, true, false);
                channel.QueueDeclare(DLX_QUEUE_NAME, true, false, false);
                channel.QueueBind(DLX_QUEUE_NAME, DLX_EXCHANGE_NAME, string.Empty, null);

                channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Direct, true, false);

                var args = new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", DLX_EXCHANGE_NAME }
                };

                channel.QueueDeclare(_queueName, true, false, false, args);

                return channel;
            }
            catch (ConnectFailureException ex)
            {
                throw new ConnectionFailedException("Connection to RabbitMQ failed", ex);
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            await ProcessMessage(eventArgs);
        }

        public async Task ProcessMessage(BasicDeliverEventArgs eventArgs)
        {
            var routingKey = eventArgs.RoutingKey;
            var serializedEnvelope = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            _logger.LogDebug($"Received message with routing key [{routingKey}]");

            try
            {
                Type messageType = _handlers[routingKey];
                Type envelopeType = typeof(Envelope<>).MakeGenericType(messageType);

                _logger.LogDebug($"Deserializing message into type {messageType.Name}");

                dynamic envelope = JsonConvert.DeserializeObject(serializedEnvelope, envelopeType);

                _logger.LogDebug($"Invoking dispatcher with message type {messageType.Name}");

                await _dispatcher.SendAsync(messageType, envelope.Context, envelope.Message);
            }
            catch (Exception ex)
            {
                _consumerChannel.BasicNack(eventArgs.DeliveryTag, false, false);

                _logger.LogError(ex, $"Failed to process received message with routing key [{routingKey}], sent to Dead Letter Exchange");

                throw;
            }

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, false);

            _logger.LogDebug($"Succesfully processed received message with routing key [{routingKey}]");
        }
    }
}
