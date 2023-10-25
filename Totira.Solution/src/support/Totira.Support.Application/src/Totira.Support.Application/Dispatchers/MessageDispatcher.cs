using LanguageExt;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Totira.Support.Application.Dispatchers.Behaviours;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;

namespace Totira.Support.Application.Dispatchers
{
    public class MessageDispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IBehaviour> _behaviours;

        public MessageDispatcher(
            IServiceProvider serviceProvider,
            IEnumerable<IBehaviour> behaviours)
        {
            _serviceProvider = serviceProvider;
            _behaviours = behaviours;
        }

        public async Task SendAsync<TMessage>(IContext context, TMessage message) where TMessage : IMessage
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<TMessage>>();

                var pipeline = _behaviours
                    .Reverse()
                    .Aggregate((BehaviourHandlerDelegate<Either<System.Exception, TMessage>>)handler.HandleAsync, (next, behaviour) => (context, message) => behaviour.HandleAsync(context, message, next));

                await pipeline(context, message);
            }
        }

        public async Task SendAsync(Type messageType, IContext context, IMessage message)
        {
            MethodInfo method = typeof(MessageDispatcher).GetMethods().Where(m => m.IsGenericMethod).First();
            MethodInfo genericMethod = method.MakeGenericMethod(messageType);

            await (Task)genericMethod.Invoke(this, new object[] { context, message });
        }
    }
}
