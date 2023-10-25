﻿using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantEmployeeIncomesCreatedEvent")]
    public class TenantEmployeeIncomesCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public string message { get; set; }

        public TenantEmployeeIncomesCreatedEvent(Guid id, string message)
        {
            Id = id;
            this.message = message;
        }

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantEmployeeIncomesCreatedInfo(this.Id,this.message);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantEmployeeIncomesCreatedInfo
        {
            private Guid id;
            public string message { get; set; }


            public TenantEmployeeIncomesCreatedInfo(Guid id, string message)
            {
                this.id = id;                
                this.message = message;
            }
        }
    }
}