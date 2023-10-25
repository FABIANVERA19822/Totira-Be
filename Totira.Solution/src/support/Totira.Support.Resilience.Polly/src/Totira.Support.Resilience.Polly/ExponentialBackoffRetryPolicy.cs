using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Totira.Support.Resilience.Polly
{
    public class ExponentialBackoffRetryPolicy : IPolicy
    {
        private readonly ILogger<ExponentialBackoffRetryPolicy> _logger;
        private readonly int _retryCount;

        internal ExponentialBackoffRetryPolicy(ILogger<ExponentialBackoffRetryPolicy> logger, int retryCount)
        {
            _logger = logger;
            _retryCount = retryCount;
        }

        public void Execute(Action action)
        {
            GetPolicy().Execute(action);
        }

        public T Execute<T>(Func<T> action)
        {
            return GetPolicy().Execute(action);
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            await GetAsyncPolicy().ExecuteAsync(action);
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            return await GetAsyncPolicy().ExecuteAsync(action);
        }

        private AsyncRetryPolicy GetAsyncPolicy()
        {
            return Policy.Handle<Exception>()
                         .WaitAndRetryAsync(
                            _retryCount,
                            attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)),
                            (exception, timeSpan, retryCount, context) =>
                                _logger.LogError($"Retrying after {timeSpan.TotalSeconds} seconds... attempt number {retryCount}"));
        }

        private RetryPolicy GetPolicy()
        {
            return Policy.Handle<Exception>()
                         .WaitAndRetry(
                            _retryCount,
                            attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)),
                            (exception, timeSpan, retryCount, context) =>
                                _logger.LogError($"Retrying after {timeSpan.TotalSeconds} seconds... attempt number {retryCount}"));
        }
    }
}
