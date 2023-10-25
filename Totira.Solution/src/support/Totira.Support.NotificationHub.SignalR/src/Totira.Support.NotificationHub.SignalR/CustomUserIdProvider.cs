using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Totira.Support.NotificationHub.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var identity = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;            

            return identity ?? string.Empty;
        }
    }
}
