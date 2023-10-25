﻿using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{


    [RoutingKey("TenantBasicInformationCreatedEvent")]
    public class TenantBasicInformationCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantBasicInformationCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantBasicInformationCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantBasicInformationCreatedInfo
        {
            private Guid id;

            public TenantBasicInformationCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }

    }
}
