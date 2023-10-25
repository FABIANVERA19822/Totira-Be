namespace Totira.Bussiness.UserService.Events.Landlord.UpdatedEvents
{
    using System;
    using Totira.Support.Application.Events;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey(nameof(UpdatePropertyClaimsFromJiraTicketEvent))]
    public class UpdatePropertyClaimsFromJiraTicketEvent : BaseValidatedEvent
    {
        public Guid Id { get; set; }
        public Guid ClaimId { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;

        public UpdatePropertyClaimsFromJiraTicketEvent()
        {
            Id = Guid.NewGuid();
        }
    }
}