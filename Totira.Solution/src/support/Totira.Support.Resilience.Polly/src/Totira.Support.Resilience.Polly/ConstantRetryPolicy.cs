using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Totira.Support.Resilience.Polly
{
    public class ConstantRetryPolicy : IPolicy
    {
        private readonly ILogger<ConstantRetryPolicy> _logger;
        private readonly int _retryCount;
        private readonly int _milliseconds;

        public ConstantRetryPolicy(ILogger<ConstantRetryPolicy> logger, int retryCount, int milliseconds)
        {
            _logger = logger;
            _retryCount = retryCount;
            _milliseconds = milliseconds;
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
                            attempt => TimeSpan.FromMilliseconds(_milliseconds),
                            (exception, timeSpan, retryCount, context) =>
                                _logger.LogError($"Retrying after {timeSpan.TotalSeconds} seconds... attempt number {retryCount}"));
        }

        private RetryPolicy GetPolicy()
        {
            return Policy.Handle<Exception>()
                         .WaitAndRetry(
                            _retryCount,
                            attempt => TimeSpan.FromMilliseconds(_milliseconds),
                            (exception, timeSpan, retryCount, context) =>
                                _logger.LogError($"Retrying after {timeSpan.TotalSeconds} seconds... attempt number {retryCount}"));
        }
    }
}
