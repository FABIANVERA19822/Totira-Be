using Microsoft.Extensions.Logging;
using Totira.Support.Application.Messages;

namespace Totira.Support.Application.Dispatchers.Behaviours
{
    public class LoggingBehaviour : IBehaviour
    {
        private readonly ILogger<LoggingBehaviour> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync<TMessage>(IContext context, TMessage message, BehaviourHandlerDelegate<TMessage> next)
            where TMessage : IMessage
        {
            _logger.LogDebug($"Message handler {typeof(TMessage).Name} execution started");

            try
            {
                await next(context, message);
            }
            catch
            {
                throw;
            }
            finally
            {
                _logger.LogDebug($"Message handler {typeof(TMessage).Name} execution finished");
            }
        }
    }
}
