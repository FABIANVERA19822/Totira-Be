namespace Totira.Support.Resilience
{
    public interface IPolicyFactory
    {
        IPolicy CreateConstantRetryPolicy(int retryCount, int miliseconds);
        IPolicy CreateExponentialBackoffRetryPolicy(int retryCount);
    }
}
