using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Totira.Support.Persistance.PostgreSQL.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPersistance<T>(this IServiceCollection services, PostgreSqlSettings settings)
            where T : DbContext
        {
            string conn = $"Host={settings.Host}; Database={settings.DatabaseName}; Username={settings.UserName}; Password={settings.Password}";
            services.AddDbContext<T>(options =>
            options.UseNpgsql(conn));
            return services;
        }
    }
}
