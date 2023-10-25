using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events;

[RoutingKey(nameof(TenantGroupVerificationDoneEvent))]
public class TenantGroupVerificationDoneEvent : IEvent
{
    public TenantGroupVerificationDoneEvent(Guid mainTenantId)
    {
        MainTenantId = mainTenantId;
    }
    public Guid MainTenantId { get; set; }
}