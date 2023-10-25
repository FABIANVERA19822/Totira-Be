using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Totira.Support.Resilience;

namespace Totira.Support.EventServiceBus.RabittMQ
{
    public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        //TODO: Move connection retries to configuration
        private const int CONNECTION_RETRIES = 3;

        private readonly IPolicyFactory _policyFactory;
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger _logger;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMQPersistentConnection(
            IPolicyFactory policyFactory,
            IConnectionFactory connectionFactory,
            ILogger<RabbitMQPersistentConnection> logger)
        {
            _policyFactory = policyFactory ?? throw new ArgumentNullException(nameof(policyFactory));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger;
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            IPolicy policy = _policyFactory.CreateExponentialBackoffRetryPolicy(CONNECTION_RETRIES);
            var model = policy.Execute(() => _connection.CreateModel());

            return model;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                if (IsConnected)
                    _connection.Dispose();

            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex, "RabbitMQ connection dispose exception");
            }
        }

        public void TryConnect()
        {
            if (_connection != null && _connection.IsOpen)
            {
                _logger.LogInformation("Connection already established");
            }

            _logger.LogDebug("RabbitMQ Client is trying to connect");

            IPolicy policy = _policyFactory.CreateExponentialBackoffRetryPolicy(CONNECTION_RETRIES);            

            _connection = policy.Execute(() => _connectionFactory.CreateConnection());

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation($"Connected to '{_connection.Endpoint.HostName}'");
            }
            else
            {
                _logger.LogCritical("Could not connect to RabbitMQ");
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            _logger.LogError(e.Exception, "RabbitMQ callback threw an exception.");
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning($"RabbitMQ connection is on shutdown. Reason: {reason.ReplyText}");

            TryConnect();
        }
    }
}
