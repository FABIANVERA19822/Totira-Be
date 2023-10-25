using System;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantUnacceptedApplicationRequestDeletedCommandEvent")]
    public class TenantUnacceptedApplicationRequestDeletedCommandEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public TenantUnacceptedApplicationRequestDeletedCommandEvent(Guid id) => Id = id;


        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantUnacceptedApplicationRequestDeletedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantUnacceptedApplicationRequestDeletedInfo
        {
            private Guid id;

            public TenantUnacceptedApplicationRequestDeletedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}

