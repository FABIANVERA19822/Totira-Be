using Microsoft.AspNetCore.SignalR;

namespace Totira.Support.NotificationHub.SignalR
{
    public class NotificationHub : INotificationHub
    {
        private readonly IHubContext<SignalRNotificationHub, IHubClient> _hubContext;

        public NotificationHub(IHubContext<SignalRNotificationHub, IHubClient> hubClient)
        {
            _hubContext = hubClient;
        }

        public async Task SendAllAsync(NotificationMessage message)
        {
            await _hubContext.Clients.All.Notify(message);
        }

        public async Task SendAsync(string userId, NotificationMessage message)
        {
            await _hubContext.Clients.User(userId).Notify(message);
        }
    }
}