using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Services.Thirdparty.Worker.Outbox.Options;
using Totira.Support.CommonLibrary.Worker;
using Totira.Support.EventServiceBus.RabittMQ;
using Totira.Support.TransactionalOutbox;

namespace Totira.Services.Thirdparty.Worker.Outbox
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMessageService _messageService;
        private readonly WorkerOptions _options;
        private readonly RabbitMQOptions _test;

        public Worker(
            ILogger<Worker> logger,
            IMessageService messageService,
            IOptions<WorkerOptions> options,
            IOptions<RabbitMQOptions> test)
        {
            _logger = logger;
            _messageService = messageService;
            _options = options.Value;
            _test = test.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {                    
                    _logger.LogDebug($"Worker about to send to process {_options.BatchSize} messages");

                    await _messageService.ProcessPendingAsync(_options.BatchSize);

                    _logger.LogDebug($"Worker process of Messages is finished");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error on ProcessPending: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(_options.Interval), stoppingToken);
            }
        }
    }
}

