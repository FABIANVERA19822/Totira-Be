using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Services.Thirdparty.Worker.Outbox.Bll.SQS;
using Totira.Services.Thirdparty.Worker.Outbox.Options;
using Totira.Support.Application.Messages;
using Totira.Services.Thirdparty.Worker.Outbox.Bll.Commands;

namespace Totira.Services.Thirdparty.Worker.Outbox.Workers
{
	public class WorkerMLSTrigger : BackgroundService
	{
        private readonly SQSEventPublisher sqsPublisher;
        private readonly IContextFactory contextFactory;
        private readonly ILogger<WorkerMLSTrigger> logger;
        private readonly WorkerMLSOptions options;


        public WorkerMLSTrigger(SQSEventPublisher sqsPublisher, IContextFactory contextFactory, ILogger<WorkerMLSTrigger> logger,
            IOptions<WorkerMLSOptions> options, ILogger<SQSEventPublisher> producerLogger)
		{
            this.sqsPublisher = sqsPublisher;
            this.contextFactory = contextFactory;
            this.logger = logger;
            this.options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("MLS Worker running at: {time}", DateTimeOffset.Now);
                await sqsPublisher.PublishAsync(contextFactory.Create(), new StartMLSDataRetrievalCommand(DateTime.UtcNow, "STARTED"));
                await Task.Delay(TimeSpan.FromMinutes(options.Interval), stoppingToken);
            }
        }
    }
}

