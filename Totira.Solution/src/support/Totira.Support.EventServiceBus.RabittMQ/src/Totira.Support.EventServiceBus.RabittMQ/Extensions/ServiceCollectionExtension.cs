using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Totira.Support.Application.Dispatchers;
using Totira.Support.Resilience;

namespace Totira.Support.EventServiceBus.RabittMQ.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, RabbitMQOptions options)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var connectionFactory = new ConnectionFactory()
                {
                    DispatchConsumersAsync = true,
                    HostName = options.HostName,
                    Password = options.Password,
                    Port = options.Port,
                    UserName = options.UserName,
                    VirtualHost = options.VirtualHost,
                };

                bool isCloudEnv = false;
                bool.TryParse(options.CloudEnv, out isCloudEnv);

                if (isCloudEnv)
                {
                    Uri uri = new Uri($"amqps://{connectionFactory.HostName}:{connectionFactory.Port}");
                    connectionFactory.Ssl = new SslOption();
                    connectionFactory.Ssl.Enabled = true;
                    connectionFactory.Uri = uri;
                }


                return new RabbitMQPersistentConnection(
                    sp.GetRequiredService<IPolicyFactory>(),
                    connectionFactory,
                    sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>());
            });

            var subscriptionClientName = options.SubscriptionClientName;

            services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();

                return new RabbitMQEventBus(rabbitMQPersistentConnection, sp.GetService<ILogger<RabbitMQEventBus>>(), sp.GetService<IDispatcher>(), subscriptionClientName);
            });

            return services;
        }
    }
}
