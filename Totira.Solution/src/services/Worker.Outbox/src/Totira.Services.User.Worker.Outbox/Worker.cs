using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Worker;

namespace Totira.Services.User.Worker.Outbox
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMessageService _messageService;
        private readonly WorkerOptions _options;

        public Worker(
            ILogger<Worker> logger,
            IMessageService messageService,
            WorkerOptions options)
        {
            _logger = logger;
            _messageService = messageService;
            _options = options;
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