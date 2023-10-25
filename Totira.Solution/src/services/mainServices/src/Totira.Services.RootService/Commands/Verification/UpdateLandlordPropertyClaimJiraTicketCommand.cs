namespace Totira.Services.RootService.Commands.Verification
{
    using Totira.Support.Application.Commands;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("UpdateLandlordPropertyClaimJiraTicketCommand")]
    public class UpdateLandlordPropertyClaimJiraTicketCommand : ICommand
    {
        public long? timestamp { get; set; }
        public string? webhookEvent { get; set; }
        public string? Issue_event_type_name { get; set; }
        public User? User { get; set; }
        public Comment? Comment { get; set; }
        public Issue? Issue { get; set; }
        public Changelog? changelog { get; set; }
    }
}