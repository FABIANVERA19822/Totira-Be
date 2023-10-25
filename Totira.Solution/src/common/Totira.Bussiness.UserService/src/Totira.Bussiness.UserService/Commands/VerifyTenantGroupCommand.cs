using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands;

[RoutingKey(nameof(VerifyTenantGroupCommand))]
public class VerifyTenantGroupCommand : ICommand
{
    public Guid MainTenantId { get; set; }
    public bool RequestVerification { get; set; }
    public bool CompleteVerification { get; set; }
    public bool RequestReVerification { get; set; }
    public bool CompleteReVerification { get; set; }
}