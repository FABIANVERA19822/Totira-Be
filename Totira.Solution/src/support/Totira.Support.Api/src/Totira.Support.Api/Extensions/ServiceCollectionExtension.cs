using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Totira.Support.Api.Connection;

namespace Totira.Support.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQueryRestClient(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IQueryRestClient, QueryRestClient>();

            return services;
        }

        public static IServiceCollection AddConfigurableOptions(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.Configure<QueryRestClientOptions>(builder.Configuration.GetSection("QueryRestClient"));

            return services;
        }


    }
}
