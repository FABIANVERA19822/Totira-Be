using Microsoft.Extensions.DependencyInjection;
using Totira.Support.Persistance.Mongo.Context.Interfaces;

namespace Totira.Support.Persistance.Mongo.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistance<T>(this IServiceCollection services)
            where T : class, IMongoDBContext
        {
            services.AddTransient<IMongoDBContext, T>();
            return services;
        }
    }
}
