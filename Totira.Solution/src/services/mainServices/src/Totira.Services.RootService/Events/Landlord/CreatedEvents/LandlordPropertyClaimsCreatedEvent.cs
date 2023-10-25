using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.Landlord.CreatedEvents
{
    [RoutingKey("LandlordPropertyClaimsCreatedEvent")]
    internal class LandlordPropertyClaimsCreatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }
        public LandlordPropertyClaimsCreatedEvent(Guid id) => Id = id;
        public NotificationMessage GetNotificationMessage()
        {
            var info = new LandlordIPropertyClaimCreatedInfo(this.Id, this.Success);
            var json = System.Text.Json.JsonSerializer.Serialize(info);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class LandlordIPropertyClaimCreatedInfo
        {
            private Guid Id;
            private bool Success;
            public LandlordIPropertyClaimCreatedInfo(Guid id, bool success)
            {
                this.Id = id;
                Success = success;
            }
        }
    }
}