﻿using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantFeedbackViaLandlordCreatedEvent")]
    public class TenantFeedbackViaLandlordCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public TenantFeedbackViaLandlordCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantFeedbackViaLandlordCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantFeedbackViaLandlordCreatedInfo
        {
            private Guid id;

            public TenantFeedbackViaLandlordCreatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}
