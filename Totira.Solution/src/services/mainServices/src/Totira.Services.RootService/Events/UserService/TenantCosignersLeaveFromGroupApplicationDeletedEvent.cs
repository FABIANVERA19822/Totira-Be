using System;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantCosignersLeaveFromGroupApplicationDeletedEvent")]
    public class TenantCosignersLeaveFromGroupApplicationDeletedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public TenantCosignersLeaveFromGroupApplicationDeletedEvent(Guid id) => Id = id;


        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantCosignersLeaveFromGroupApplicationDeletedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantCosignersLeaveFromGroupApplicationDeletedInfo
        {
            private Guid id;

            public TenantCosignersLeaveFromGroupApplicationDeletedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}

