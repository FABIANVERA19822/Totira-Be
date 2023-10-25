using System;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Totira.Services.Thirdparty.Worker.Outbox.Options;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.EventServiceBus.Exceptions;

namespace Totira.Services.Thirdparty.Worker.Outbox.Bll.SQS
{
    public class SQSEventPublisher : IEventPublisher
    {
        private const string RoutingKey = "RoutingKey";

        
        private readonly SQSOptions sqsOptions;
        private readonly ILogger<SQSEventPublisher> logger;

        public SQSEventPublisher(IOptions<SQSOptions> sqsOptions, ILogger<SQSEventPublisher> logger)
        {
            this.logger = logger;
            this.sqsOptions = sqsOptions.Value;
        }

        public async Task PublishAsync(IContext context, IMessage message)
        {
            var messageType = message.GetType();
            var routingKey = CustomAttributeExtensions.GetRoutingKey(messageType);

            if (routingKey == null)
                throw new MissingRoutingKeyException(message.GetType());

            logger.LogInformation("Publish a message in queue: {QueueName} with {RoutingKey}", sqsOptions.QueueName, routingKey);
            var serializedEnvelope = JsonConvert.SerializeObject(new Envelope<IMessage>(context, message));
            await PublishAsync(serializedEnvelope, sqsOptions.QueueName, routingKey);
        }

        public async Task PublishAsync(string messageBody, string routingKey)
        {
            if (string.IsNullOrEmpty(routingKey))
            {
                logger.LogError("Publish a message to the queue failed. Routing key is null or empty");
                throw new MissingRoutingKeyException();
            }

            logger.LogInformation("Publish a message in queue: {QueueName} with {RoutingKey}", sqsOptions.QueueName, routingKey);
            await PublishAsync(messageBody, sqsOptions.QueueName, routingKey);
        }

        private async Task PublishAsync(string serializedMessageBody, string queueName, string routingKey)
        {
            await Task.Yield();

            var sendMessageRequest = new SendMessageRequest
            {
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    { RoutingKey, new MessageAttributeValue { DataType = "String", StringValue = routingKey } }
                },
                MessageBody = serializedMessageBody,
                QueueUrl = GetQueueUrl(queueName),
                MessageGroupId = routingKey,
                MessageDeduplicationId = Guid.NewGuid().ToString()
            };
            logger.LogInformation("Sending message to {QueueUrl}", sendMessageRequest.QueueUrl);
            var sqsClient = CreateSQSClient();
            var response = await sqsClient.SendMessageAsync(sendMessageRequest);
            logger.LogInformation("Sent a message with id : {MessageId}", response.MessageId);
        }

        private IAmazonSQS CreateSQSClient()
        {
            if (sqsOptions.UseLocalStack)
            {
                return new AmazonSQSClient(
                    new AmazonSQSConfig
                    {
                        ServiceURL = sqsOptions.ServiceUrl,
                        UseHttp = true
                    });
            }

            if (string.IsNullOrEmpty(sqsOptions.AccessKey) || string.IsNullOrEmpty(sqsOptions.SecretKey))
                return new AmazonSQSClient(RegionEndpoint.GetBySystemName(sqsOptions.Region));

            var credentials = new BasicAWSCredentials(sqsOptions.AccessKey, sqsOptions.SecretKey);
            return new AmazonSQSClient(credentials, RegionEndpoint.GetBySystemName(sqsOptions.Region));
        }

        private string GetQueueUrl(string queueName) => sqsOptions.UseLocalStack ?
           $"{sqsOptions.ServiceUrl}/{sqsOptions.AccountId}/{queueName}" :
           $"https://sqs.{sqsOptions.Region}.amazonaws.com/{sqsOptions.AccountId}/{queueName}";
    }


}

