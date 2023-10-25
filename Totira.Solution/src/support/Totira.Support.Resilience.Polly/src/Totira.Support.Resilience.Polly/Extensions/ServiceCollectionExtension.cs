using Microsoft.Extensions.DependencyInjection;

namespace Totira.Support.Resilience.Polly.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddPolicyFactory(this IServiceCollection services)
        {
            services.AddTransient<IPolicyFactory, PolicyFactory>();

            return services;
        }
    }
}
