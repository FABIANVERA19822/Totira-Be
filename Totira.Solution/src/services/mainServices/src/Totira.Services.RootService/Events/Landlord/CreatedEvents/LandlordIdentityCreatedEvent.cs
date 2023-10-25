using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.Landlord.CreatedEvents
{
    [RoutingKey("LandlordIdentityCreatedEvent")]
    public class LandlordIdentityCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }

        public LandlordIdentityCreatedEvent(Guid id, bool success) {Id = id; Success = success; }

        public NotificationMessage GetNotificationMessage()
        {
            var info = new LandlordIdentityInformationCreatedInfo(this.Id, this.Success);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class LandlordIdentityInformationCreatedInfo
        {
            private Guid id { get; set; }
            private bool success { get; set; }

            public LandlordIdentityInformationCreatedInfo(Guid id, bool success)
            {
                this.id = id;
                this.success = success;
            }
        }
    }
}
