using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Totira.Bussiness.UserService.Context;
using Totira.Bussiness.UserService.Extensions;
using Totira.Bussiness.UserService.Repositories;
using Totira.Support.CommonLibrary.Worker;
using Totira.Support.EventServiceBus.RabittMQ;
using Totira.Support.EventServiceBus.RabittMQ.Extensions;
using Totira.Support.Persistance.Mongo;
using Totira.Support.Persistance.Mongo.Extensions;
using Totira.Support.Resilience.Polly.Extensions;
using Totira.Support.TransactionalOutbox.Extensions;

namespace Totira.Services.User.Worker.Outbox
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

                services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));


                WorkerOptions options = configuration.GetSection("WorkerOptions").Get<WorkerOptions>();
                services.AddSingleton(options);

                services.AddPersistance<MongoDBContext>();

                services.AddTransactionalOutbox<MessageOutboxRepository>();

                services.AddPolicyFactory();

                // Add Repositories
                services.AddRepositories();

                // Add Persistance
                services.AddPersistance<MongoDBContext>();


                services.AddEventBus(configuration.GetSection("EventBus:RabbitMQ").Get<RabbitMQOptions>());

                services.AddHostedService<Worker>(); 
            });
    }
}