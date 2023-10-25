namespace Totira.Bussiness.UserService.Commands.LandlordCommands
{
    using Totira.Support.Application.Commands;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey(nameof(UpdatePropertyClaimsFromJiraTicketCommand))]
    public class UpdatePropertyClaimsFromJiraTicketCommand : ICommand
    {
        public Guid LandlordId { get; init; }
        public Guid ClaimId { get; init; }
        public string MLSId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
    }
}