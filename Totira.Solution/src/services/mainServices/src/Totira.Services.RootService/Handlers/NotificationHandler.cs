using LanguageExt;
using Totira.Support.Application.Events;
using Totira.Support.Application.Messages;
using Totira.Support.NotificationHub;
using static Totira.Support.Application.Messages.IMessageHandler;

namespace Totira.Services.RootService.Handlers
{
    public class NotificationHandler<TEvent> : IMessageHandler<TEvent>
        where TEvent : IEvent, INotification
    {
        private readonly ILogger<NotificationHandler<TEvent>> _logger;
        private readonly INotificationHub _notificationHub;

        public NotificationHandler(
            ILogger<NotificationHandler<TEvent>> logger,
            INotificationHub notificationHub)
        {
            _logger = logger;
            _notificationHub = notificationHub;
        }

        public async Task HandleAsync(IContext context, Either<Exception, TEvent> message)
        {
            await message.MatchAsync(async msg => {
                _logger.LogDebug("Getting notification Message for event of type {EventType}", typeof(TEvent).Name);

                var notification = msg.GetNotificationMessage();

                if (context.CreatedBy == Guid.Empty)
                {
                    _logger.LogDebug("Discarted notification (no destination user) with status '{NotificationStatus}' and Message '{NotificationMessage}'", notification.Status, notification.Message);
                }
                else
                {
                    _logger.LogDebug("Sending notification to user {NotificationCreatedBy} with status '{NotificationStatus}' and Message '{NotificationMessage}'", context.CreatedBy, notification.Status, notification.Message);
                    notification.Event = typeof(TEvent).Name;
                    await _notificationHub.SendAsync(context.CreatedBy.ToString(), notification);
                }
            }, ex => throw ex);
        }
    }
}