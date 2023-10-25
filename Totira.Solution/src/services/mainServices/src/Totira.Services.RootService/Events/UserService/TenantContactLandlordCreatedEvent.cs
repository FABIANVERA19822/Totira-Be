using System;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantContactLandlordCreatedEvent")]
    public class TenantContactLandlordCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantContactLandlordCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantContactLandlordCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }
        private class TenantContactLandlordCreatedInfo
        {
            private Guid id;

            public TenantContactLandlordCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}

