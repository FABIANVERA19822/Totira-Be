using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Totira.Support.NotificationHub.SignalR
{
    [Authorize]
    public class SignalRNotificationHub : Hub<IHubClient>
    {
        private readonly ILogger<SignalRNotificationHub> _logger;

        public SignalRNotificationHub(ILogger<SignalRNotificationHub> logger) : base()
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogDebug($"Client connected with id {Context.ConnectionId}");
            var httpContext = Context.GetHttpContext();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogDebug($"Client disconnected with id {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
