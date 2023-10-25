using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Totira.Support.Resilience.Polly
{
    public class PolicyFactory : IPolicyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PolicyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPolicy CreateConstantRetryPolicy(int retryCount, int milliseconds)
        {
            if (retryCount < 1)
                throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "Value must be a positive integer");

            if (milliseconds < 1)
                throw new ArgumentOutOfRangeException(nameof(milliseconds), milliseconds, "Value must be a positive integer");

            var logger = _serviceProvider.GetRequiredService<ILogger<ConstantRetryPolicy>>();

            return new ConstantRetryPolicy(logger, retryCount, milliseconds);
        }

        public IPolicy CreateExponentialBackoffRetryPolicy(int retryCount)
        {
            if (retryCount < 1)
                throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "Value must be a positive integer");

            var logger = _serviceProvider.GetRequiredService<ILogger<ExponentialBackoffRetryPolicy>>();

            return new ExponentialBackoffRetryPolicy(logger, retryCount);
        }


    }
}