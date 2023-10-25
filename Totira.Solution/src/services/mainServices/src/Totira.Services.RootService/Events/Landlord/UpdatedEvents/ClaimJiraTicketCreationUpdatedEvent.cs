using Newtonsoft.Json;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.Landlord.UpdatedEvents
{
    [RoutingKey("ClaimJiraTicketCreationUpdatedEvent")]
    public class ClaimJiraTicketCreationUpdatedEvent:IEvent, INotification
    {
        public Guid Id { get; set; }

        public ClaimJiraTicketCreationUpdatedEvent(Guid id) => Id = id;

        public ClaimJiraTicketCreationUpdatedEvent()
        {
        }

        public NotificationMessage GetNotificationMessage()
        {
            var response = new ClaimJiraTicketCreationUpdatedResponse(Id);
            var json = JsonConvert.SerializeObject(response);
            return new NotificationMessage(NotificationMessageStatus.Success, json);
        }

        private class ClaimJiraTicketCreationUpdatedResponse
        {
            [JsonProperty]
            public Guid ClaimId { get; set; }

            public ClaimJiraTicketCreationUpdatedResponse(Guid claimId)
            {
                ClaimId = claimId;
            }
        }
    }
}
