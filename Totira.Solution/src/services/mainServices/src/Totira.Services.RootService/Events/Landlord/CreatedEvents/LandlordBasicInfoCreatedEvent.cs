using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.Landlord.CreatedEvents
{
    [RoutingKey("LandlordBasicInformationCreatedEvent")]
    public class LandlordBasicInformationCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public LandlordBasicInformationCreatedEvent(Guid id) => Id = id;

        public NotificationMessage GetNotificationMessage()
        {
            var info = new LandlordBasicInformationCreatedInfo(this.Id);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class LandlordBasicInformationCreatedInfo
        {
            private Guid id;

            public LandlordBasicInformationCreatedInfo(Guid id)
            {
                this.id = id;
            }
        }

    }
}
