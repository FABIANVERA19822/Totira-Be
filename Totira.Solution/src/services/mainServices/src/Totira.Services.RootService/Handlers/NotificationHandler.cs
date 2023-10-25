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

        public async Task HandleAsync(IContext context, TEvent message)
        {
            _logger.LogDebug($"Getting notification message for event of type {typeof(TEvent).Name}");

            var notification = message.GetNotificationMessage();

            if (context.CreatedBy == Guid.Empty)
            {
                _logger.LogDebug($"Discarted notification (no destination user) with status '{notification.Status}' and message '{notification.Message}'");
            }
            else
            {
                _logger.LogDebug($"Sending notification to user {context.CreatedBy} with status '{notification.Status}' and message '{notification.Message}'");

                await _notificationHub.SendAsync(context.CreatedBy.ToString(), notification);
            }
        }
    }
}
