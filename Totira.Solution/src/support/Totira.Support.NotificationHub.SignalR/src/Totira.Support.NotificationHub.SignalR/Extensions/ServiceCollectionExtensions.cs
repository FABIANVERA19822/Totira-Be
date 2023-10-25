using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Totira.Support.NotificationHub.SignalR.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSignalRNotificationHub(this IServiceCollection services)
        {
            services.AddTransient<INotificationHub, NotificationHub>();

            services.AddSignalR(
#if DEBUG
                options =>
                {
                    options.EnableDetailedErrors = true;
                }
#endif
                ).AddJsonProtocol();
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            return services;
        }
    }
}
