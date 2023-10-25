using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events.Landlord.UpdatedEvents
{
    [RoutingKey("ClaimJiraTicketCreationUpdatedEvent")]
    public class ClaimJiraTicketCreationUpdatedEvent: BaseValidatedEvent
    {
        public Guid Id { get; set; }
        public ClaimJiraTicketCreationUpdatedEvent(Guid id) => Id = id;
        public ClaimJiraTicketCreationUpdatedEvent()
        {
        }
    }
}
