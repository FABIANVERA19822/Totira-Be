using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events.Landlord.UpdatedEvents
{
    [RoutingKey("ClaimJiraTicketResultUpdatedEvent")]
    public class ClaimJiraTicketResultUpdatedEvent : BaseValidatedEvent
    {
        public Guid Id { get; set; }
        public ClaimJiraTicketResultUpdatedEvent(Guid id) => Id = id;
        public ClaimJiraTicketResultUpdatedEvent()
        {
        }
    }
}
