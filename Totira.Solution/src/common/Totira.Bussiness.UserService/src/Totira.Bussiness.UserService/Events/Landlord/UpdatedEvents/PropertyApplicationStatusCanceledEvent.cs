using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Events.Landlord.UpdatedEvents;

[RoutingKey(nameof(PropertyApplicationStatusCanceledEvent))]
public class PropertyApplicationStatusCanceledEvent : BaseValidatedEvent
{
    /// <summary>
    /// Property application identifier
    /// </summary>
    public Guid PropertyApplicationId { get; set; }

    public PropertyApplicationStatusCanceledEvent(Guid propertyApplicationId) => PropertyApplicationId = propertyApplicationId;
    
    public PropertyApplicationStatusCanceledEvent()
    {
    }
}
