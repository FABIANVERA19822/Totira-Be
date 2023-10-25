using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Totira.Business.ThirdPartyIntegrationService.Context;
using Totira.Business.ThirdPartyIntegrationService.Extensions;
using Totira.Business.ThirdPartyIntegrationService.Repositories;
using Totira.Services.Thirdparty.Worker.Outbox.Bll.SQS;
using Totira.Services.Thirdparty.Worker.Outbox.Options;
using Totira.Services.Thirdparty.Worker.Outbox.Workers;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Extensions;
using Totira.Support.Api.Options;
using Totira.Support.Application;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Configurations;
using Totira.Support.CommonLibrary.Extensions;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.CommonLibrary.Worker;
using Totira.Support.EventServiceBus.RabittMQ;
using Totira.Support.EventServiceBus.RabittMQ.Extensions;
using Totira.Support.Persistance.Mongo;
using Totira.Support.Persistance.Mongo.Extensions;
using Totira.Support.Resilience.Polly.Extensions;
using Totira.Support.TransactionalOutbox.Extensions;

namespace Totira.Services.Thirdparty.Worker.Outbox
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuration = hostContext.Configuration;

                services.Configure<SesSettings>(configuration.GetSection("SesSettings"));
                services.Configure<FrontendSettings>(configuration.GetSection("FrontendSettings"));
                services.Configure<WorkerCertnOptions>(configuration.GetSection("WorkerCertnOptions"));
                services.Configure<WorkerVerifiedProfileOptions>(configuration.GetSection("WorkerVerifiedProfileOptions"));
                services.Configure<WorkerOptions>(configuration.GetSection("WorkerOptions"));
                services.Configure<RestClientOptions>(configuration.GetSection("RestClient"));
                services.Configure<QueryRestClientOptions>(configuration.GetSection("QueryRestClient"));
                services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));
                services.Configure<RabbitMQOptions>(configuration.GetSection("EventBus:RabbitMQ"));
                services.Configure<WorkerMLSOptions>(configuration.GetSection("WorkerMLSOptions"));
                services.Configure<SQSOptions>(configuration.GetSection(SQSOptions.SectionName));


                services.AddCommonLibrary();
                services.AddQueryRestClient();
                services.AddPolicyFactory();

                services.AddTransient<IDateTimeProvider, DateTimeProvider>();
                services.AddTransient<IContextFactory, ContextFactory>();

                // Add Persistance
                services.AddPersistance<MongoDBContext>();

                services.AddTransactionalOutbox<MessageOutboxRepository>();

                // Add Repositories
                services.AddRepositories();

                var configRabbit = configuration.GetSection("EventBus:RabbitMQ").Get<RabbitMQOptions>();                

                services.AddEventBus(configRabbit);

                services.AddHostedService<Worker>();
                services.AddHostedService<WorkerCertn>();
                services.AddHostedService<WorkerVerifiedProfile>();
                services.AddHostedService<WorkerMLSTrigger>();
                services.AddSingleton<SQSEventPublisher>();
            });
    }
}