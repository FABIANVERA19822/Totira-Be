using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Totira.Support.Application.Messages;
using ValidationResult = Totira.Support.Application.Messages.ValidationResult;

namespace Totira.Support.Application.Dispatchers.Behaviours
{
    public class ValidationBehaviour : IBehaviour
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ValidationBehaviour> _logger;

        public ValidationBehaviour(
            IServiceProvider serviceProvider,
            ILogger<ValidationBehaviour> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }


        public async Task HandleAsync<TMessage>(IContext context, TMessage message, BehaviourHandlerDelegate<TMessage> next)
            where TMessage : IMessage
        {
            var validator = _serviceProvider.GetService<IMessageValidator<TMessage>>();

            if (validator == null)
            {
                _logger.LogWarning($"No validator registered for message {typeof(TMessage).Name}");
            }
            else
            {
                ValidationResult result = validator.Validate(message);

                if (result.IsValid)
                {
                    _logger.LogDebug($"Message {typeof(TMessage).Name} is valid");
                }
                else
                {
                    _logger.LogWarning($"Message {typeof(TMessage).Name} is not valid and its handler will not be executed");

                    throw new Totira.Support.Application.Exception.ValidationException(message, result.Errors);
                }
            }

            await next(context, message);
        }
    }
}
