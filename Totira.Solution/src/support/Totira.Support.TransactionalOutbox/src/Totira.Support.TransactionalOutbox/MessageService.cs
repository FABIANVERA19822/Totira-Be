using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.EventServiceBus.Exceptions;

namespace Totira.Support.TransactionalOutbox
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IEventBus _bus;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IMessageRepository messageRepository, IEventBus bus, ILogger<MessageService> logger)
        {
            _messageRepository = messageRepository;
            _bus = bus;
            _logger = logger;
        }

        public async Task ProcessAsync(Guid messageId)
        {
            _logger.LogDebug($"Processing message with id {messageId}");

            var message = await _messageRepository.GetAsync(messageId);

            if (message == null)
            {
                _logger.LogWarning($"Message with id {messageId} was not found");
            }

            await this.ProcessAsync(message);
        }

        public async Task ProcessPendingAsync(int count)
        {
            var messagesToProcess = await _messageRepository.GetPendingAsync(count);

            _logger.LogDebug($"About to process {messagesToProcess.Count()} messages");

            foreach (var message in messagesToProcess)
            {
                _logger.LogDebug($"Starting to process message with id {message.Id}");

                await ProcessAsync(message);

                _logger.LogDebug($"Finished processing message with id {message.Id}");
            }
        }

        private async Task ProcessAsync(IPersistedMessage message)
        {
            await _bus.PublishAsync(message.SerializedBody, message.RoutingKey);

            await _messageRepository.DeleteAsync(message);
            await _messageRepository.CommitAsync();
        }

        public async Task<Guid> SendAsync(IContext context, IMessage message)
        {
            var routingKey = CustomAttributeExtensions.GetRoutingKey(message.GetType());

            if (routingKey == null)
                throw new MissingRoutingKeyException(message.GetType());

            _logger.LogDebug($"Sending message with id {context.TransactionId} and routing key {routingKey} to outbox");

            var serializedEnvelope = JsonConvert.SerializeObject(new Envelope<IMessage>(context, message));

            _logger.LogDebug($"Message with id {context.TransactionId} was serialized");

            var messageOutbox = new MessageOutbox(
                context.TransactionId,
                serializedEnvelope,
                routingKey,
                DateTimeOffset.Now
                );

            await _messageRepository.AddAsync(messageOutbox);

            _logger.LogDebug($"Message with id {context.TransactionId} and routing key {routingKey} added to repository");

            return context.TransactionId;
        }
    }
}
