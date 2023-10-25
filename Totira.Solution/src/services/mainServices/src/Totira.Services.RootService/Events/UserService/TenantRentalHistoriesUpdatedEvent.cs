using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService
{
    [RoutingKey("TenantRentalHistoriesUpdatedEvent")]
    public class TenantRentalHistoriesUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public TenantRentalHistoriesUpdatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new TenantRentalHistoriesUpdatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class TenantRentalHistoriesUpdatedInfo
        {
            private Guid id;

            public TenantRentalHistoriesUpdatedInfo(Guid id)
            {
                this.id = id;

            }
        }
    }
}

