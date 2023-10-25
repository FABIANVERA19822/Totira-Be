using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Events.Jira
{
    [RoutingKey("CreateLandlordPropertyClaimsJiraTicketEvent")]
    public class CreateLandlordPropertyClaimsJiraTicketEvent : BaseValidatedEvent
    {
        public Guid Id { get; set; }
        public string Message { get; set; }

        public CreateLandlordPropertyClaimsJiraTicketEvent()
        {
            this.Id = Guid.Empty; 
            this.Message = string.Empty;
        }

        public CreateLandlordPropertyClaimsJiraTicketEvent(Guid id, string Message)
        {
            this.Id = id;
            this.Message = Message;
        }
    }
}