namespace Totira.Business.ThirdPartyIntegrationService.Events.Jira
{
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("LandlordPropertyClaimJiraTicketUpdatedEvent")]
    public class LandlordPropertyClaimJiraTicketUpdatedEvent : BaseValidatedEvent
    {
        public Guid Id { get; set; }

        public LandlordPropertyClaimJiraTicketUpdatedEvent(Guid id) => Id = id;

        public LandlordPropertyClaimJiraTicketUpdatedEvent()
        {
            Id = Guid.Empty;
        }
    }
}