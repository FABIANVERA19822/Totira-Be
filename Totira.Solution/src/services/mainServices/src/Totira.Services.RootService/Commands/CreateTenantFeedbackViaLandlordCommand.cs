using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("CreateTenantFeedbackViaLandlordCommand")]
    public class CreateTenantFeedbackViaLandlordCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public Guid LandlordId { get; set; }
        public int StarScore { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
