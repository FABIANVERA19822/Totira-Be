using LanguageExt;
using Microsoft.Extensions.Logging;
using Totira.Support.Application.Events;
using Totira.Support.Application.Extensions;
using static Totira.Support.Application.Messages.IMessageHandler;

namespace Totira.Support.Application.Messages;

public abstract class BaseMessageHandler<TMessage, TResultEvent> : IMessageHandler<TMessage> where TMessage : IMessage where TResultEvent : BaseValidatedEvent, new()
{
    protected readonly ILogger _logger;
    protected readonly IContextFactory _contextFactory;
    protected readonly IMessageService _messageService;

    protected BaseMessageHandler(ILogger logger, IContextFactory contextFactory, IMessageService messageService)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _messageService = messageService;
    }

    public virtual async Task HandleAsync(IContext context, Either<System.Exception, TMessage> command)
    {
        TResultEvent? resultEvent = null;
        try
        {
            resultEvent = await command.MatchAsync(async cmd => await Process(context, cmd),
                ex => {
                    _logger.LogError(ex, "An error occurred while executing command: {CommandName}", typeof(TMessage).Name);
                    return ex.CreateValidatedEventOf<TResultEvent>($"An error occurred while executing command: {typeof(TMessage).Name}");
                });
        } catch(System.Exception ex)
        {
            _logger.LogError(ex, "An error occurred while executing command: {CommandName}", typeof(TMessage).Name);
            resultEvent = ex.CreateValidatedEventOf<TResultEvent>($"An error occurred while executing command: {typeof(TMessage).Name}");
        }
        finally
        {
            if(resultEvent is not null)
                await NotifyEvent(context, resultEvent);    
        }
    }

    protected abstract Task<TResultEvent> Process(IContext context, TMessage command);

    protected async Task NotifyEvent(IContext context, IEvent resultEvent)
    {
        var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
        await _messageService.SendAsync(notificationContext, resultEvent);
    }
}