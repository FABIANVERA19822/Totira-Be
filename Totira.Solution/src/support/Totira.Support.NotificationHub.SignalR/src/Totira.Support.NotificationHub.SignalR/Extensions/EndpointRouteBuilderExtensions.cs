


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Totira.Support.NotificationHub.SignalR.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapNotificationHub(this IEndpointRouteBuilder routes, string pattern)
        {
            return routes.MapHub<SignalRNotificationHub>(pattern);
        }


    }
}
