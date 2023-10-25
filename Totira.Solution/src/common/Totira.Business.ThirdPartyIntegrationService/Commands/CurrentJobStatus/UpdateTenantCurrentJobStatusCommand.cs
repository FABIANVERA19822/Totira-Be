
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Commands.CurrentJobStatus
{
    [RoutingKey("UpdateTenantCurrentJobStatusCommand")]
    public class UpdateTenantCurrentJobStatusCommand : ICommand
    {
        public bool UpdateCurrentJobStatus { get; set; } = true;
        public Guid TenantId { get; set; }
        public string CurrentJobStatus { get; set; } = string.Empty;
        public bool IsUnderRevisionSend { get; set; }
    }
}
