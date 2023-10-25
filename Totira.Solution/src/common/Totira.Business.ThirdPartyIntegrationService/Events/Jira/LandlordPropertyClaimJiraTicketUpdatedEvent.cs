using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Events.Jira
{
    [RoutingKey("LandlordPropertyClaimJiraTicketUpdatedEvent")]
    public class LandlordPropertyClaimJiraTicketUpdatedEvent : IEvent
    {
        public Guid Id { get; set; }

        public LandlordPropertyClaimJiraTicketUpdatedEvent(Guid id) => Id = id;

    }
}
