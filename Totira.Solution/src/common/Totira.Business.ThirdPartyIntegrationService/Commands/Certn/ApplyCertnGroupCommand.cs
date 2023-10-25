using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.ThirdPartyIntegration.Certn.Model.PropertyManagement;

namespace Totira.Business.ThirdPartyIntegrationService.Commands.Certn;

[RoutingKey(nameof(ApplyCertnGroupCommand))]
public class ApplyCertnGroupCommand : ICommand
{
    public List<SingleCoapplicant> TenantGroups { get; set; } = new();

    public class SingleCoapplicant
    {
        public Guid TenantId { get; set; }
        public SoftCheckRequestModel RequestModel { get; set; } = default!;
    }
}