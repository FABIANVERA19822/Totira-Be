using Microsoft.Extensions.DependencyInjection;
using Totira.Support.Application.Messages;

namespace Totira.Support.TransactionalOutbox.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddTransactionalOutbox<TRepository>(this IServiceCollection services)
            where TRepository : class, IMessageRepository
        {
            services.AddTransient<IMessageRepository, TRepository>();
            services.AddTransient<IMessageService, MessageService>();
            return services;
        }
    }
}
