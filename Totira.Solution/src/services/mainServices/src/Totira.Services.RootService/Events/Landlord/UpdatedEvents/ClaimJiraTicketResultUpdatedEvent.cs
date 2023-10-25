using Newtonsoft.Json;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.Landlord.UpdatedEvents
{
    [RoutingKey("ClaimJiraTicketResultUpdatedEvent")]
    public class ClaimJiraTicketResultUpdatedEvent : IEvent, INotification
    {
        public Guid Id { get; set; }

        public ClaimJiraTicketResultUpdatedEvent(Guid id) => Id = id;

        public ClaimJiraTicketResultUpdatedEvent()
        {
        }

        public NotificationMessage GetNotificationMessage()
        {
            var response = new ClaimJiraTicketResultUpdatedResponse(Id);
            var json = JsonConvert.SerializeObject(response);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class ClaimJiraTicketResultUpdatedResponse
        {
            [JsonProperty]
            public Guid ClaimId { get; set; }

            public ClaimJiraTicketResultUpdatedResponse(Guid claimId)
            {
                ClaimId = claimId;
            }
        }
    }
}
