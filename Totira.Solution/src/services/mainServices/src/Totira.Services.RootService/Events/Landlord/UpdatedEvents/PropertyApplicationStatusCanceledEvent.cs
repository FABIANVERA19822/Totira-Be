using Newtonsoft.Json;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.Landlord.UpdatedEvents;

[RoutingKey(nameof(PropertyApplicationStatusCanceledEvent))]
public class PropertyApplicationStatusCanceledEvent : IEvent, INotification
{
    /// <summary>
    /// Property application identifier
    /// </summary>
    public Guid PropertyApplicationId { get; set; }

    public PropertyApplicationStatusCanceledEvent(Guid propertyApplicationId) => PropertyApplicationId = propertyApplicationId;
    
    public PropertyApplicationStatusCanceledEvent()
    {
    }

    public NotificationMessage GetNotificationMessage()
    {
        var response = new PropertyApplicationApprovedOrRejectedCanceledResponse(PropertyApplicationId);
        var json = JsonConvert.SerializeObject(response);
        return new NotificationMessage(NotificationMessageStatus.Success, json);
    }

    private class PropertyApplicationApprovedOrRejectedCanceledResponse
    {
        [JsonProperty]
        public Guid PropertyApplicationId { get; set; }

        public PropertyApplicationApprovedOrRejectedCanceledResponse(Guid propertyApplicationId)
        {
            PropertyApplicationId = propertyApplicationId;
        }
    }
}
