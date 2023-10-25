namespace Totira.Bussiness.UserService.Commands.LandlordCommands.Update
{
    using Totira.Support.Application.Commands;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("UpdateClaimJiraTicketResultCommand")]
    public class UpdateClaimJiraTicketResultCommand : ICommand
    {
        public Guid ClaimId { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string MLSId { get; set; }
    }
}