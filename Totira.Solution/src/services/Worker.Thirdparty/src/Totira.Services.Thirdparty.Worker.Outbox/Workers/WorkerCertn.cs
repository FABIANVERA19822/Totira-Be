using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Services.Thirdparty.Worker.Outbox.Bll.DTO;
using Totira.Services.Thirdparty.Worker.Outbox.Options;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;

namespace Totira.Services.Thirdparty.Worker.Outbox.Workers
{
    public class WorkerCertn : BackgroundService
    {
        private readonly ILogger<WorkerCertn> _logger;
        private readonly WorkerCertnOptions _options;
        private readonly RestClientOptions _restClientOptions;
        private readonly IQueryRestClient _queryRestClient;


        public WorkerCertn(
            IQueryRestClient queryRestClient,
            IOptions<RestClientOptions> restClientOptions,
            IOptions<WorkerCertnOptions> workerCertnOptions,
            ILogger<WorkerCertn> logger            
            )
        {
            _queryRestClient = queryRestClient;
            _restClientOptions = restClientOptions.Value;
            _options = workerCertnOptions.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Certn Worker running at: {time}", DateTimeOffset.Now);

                    var response = await _queryRestClient.GetAsync<ListTenantApplicationDto>($"{_restClientOptions.ThirdPartyIntegration}/Certn/applicants/");
                    var result = response.Content;

                    if (result.Count > 0)
                    {
                        foreach (var process in result.TenantApplications)
                        {
                            _logger.LogDebug($"Starting to process message with id {process.Id}");

                            var resultInfo = await _queryRestClient.GetAsync<GetCertnApplicationDto>($"{_restClientOptions.ThirdPartyIntegration}/Certn/applicants/{process.Id}");

                            _logger.LogDebug($"Finished processing message with id {process.Id}");
                        }
                    }
                    _logger.LogDebug("Certn Worker process of Messages is finished at : {time}", DateTimeOffset.Now);
                }
                catch (Exception exc)
                {
                    _logger.LogError($"Error on Certn Worker ProcessPending: {exc.Message}", exc);
                }
                await Task.Delay(TimeSpan.FromMinutes(_options.Interval), stoppingToken);
            }
        }
    }
}
