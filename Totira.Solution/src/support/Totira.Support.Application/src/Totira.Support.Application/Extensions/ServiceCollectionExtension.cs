using Microsoft.Extensions.DependencyInjection;
using Totira.Support.Application.Dispatchers;
using Totira.Support.Application.Dispatchers.Behaviours;
using Totira.Support.Application.Events;
using Totira.Support.Application.Messages;
using Totira.Support.Application.Queries;
using Totira.Support.Application.Transactions;
using static Totira.Support.Application.Messages.IMessageHandler;

namespace Totira.Support.Application.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddCommandHandler<TCommand, TCommandHandler>(this IServiceCollection services)
            where TCommand : Totira.Support.Application.Commands.ICommand
            where TCommandHandler : class, IMessageHandler<TCommand>
        {
            services.AddTransient<IMessageHandler<TCommand>, TCommandHandler>();

            return services;
        }


        public static IServiceCollection AddCommandHandler<TCommand, TCommandHandler, TCommandValidator>(this IServiceCollection services)
            where TCommand : Totira.Support.Application.Commands.ICommand
            where TCommandHandler : class, IMessageHandler<TCommand>
            where TCommandValidator : class, IMessageValidator<TCommand>
        {
            services.AddTransient<IMessageHandler<TCommand>, TCommandHandler>();
            services.AddTransient<IMessageValidator<TCommand>, TCommandValidator>();

            return services;
        }


        public static IServiceCollection AddEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
            where TEvent : IEvent
            where TEventHandler : class, IMessageHandler<TEvent>
        {
            services.AddTransient<IMessageHandler<TEvent>, TEventHandler>();
            services.AddTransient<TEventHandler>();

            return services;
        }


        public static IServiceCollection AddQueryHandler<TRequest, TResponse, TQueryHandler>(this IServiceCollection services)
            where TRequest : IQuery
            where TQueryHandler : class, IQueryHandler<TRequest, TResponse>
        {
            services.AddTransient<IQueryHandler<TRequest, TResponse>, TQueryHandler>();

            return services;
        }


        public static IServiceCollection AddMessageDispatcher(this IServiceCollection services)
        {
            services.AddTransient<IDispatcher>(s =>
            {
                return (new DispatcherFactory(s))
                .AddBehaviour<LoggingBehaviour>()
                .AddBehaviour<ValidationBehaviour>()
                .Create();
            });

            return services;
        }


        public static IServiceCollection AddContextFactory(this IServiceCollection services)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IContextFactory, ContextFactory>();

            return services;
        }


        public static IServiceCollection AddIdempotency<TRepository>(this IServiceCollection services)
          where TRepository : class, ITransactionRepository
        {
            services.AddTransient<ITransactionRepository, TRepository>();
            services.AddTransient<IIdempotencyService, IdempotencyService>();
            return services;
        }
    }


}
