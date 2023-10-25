using Newtonsoft.Json;
using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.Landlord.UpdatedEvents;
[RoutingKey("ApplicationRequestRejectedEvent")]
public class ApplicationRequestRejectedEvent : IEvent, INotification
{
    public Guid Id { get; set; }

    public ApplicationRequestRejectedEvent(Guid id) => Id = id;

    public ApplicationRequestRejectedEvent()
    {
    }
    
    public NotificationMessage GetNotificationMessage()
    {
        var response = new ApplicationRequestApprovedResponse(Id);
        var json = JsonConvert.SerializeObject(response);
        return new NotificationMessage(NotificationMessageStatus.Success, json);
    }
    
    private class ApplicationRequestApprovedResponse
    {
        [JsonProperty]
        public Guid PropertyApplicationId { get; set; }

        public ApplicationRequestApprovedResponse(Guid propertyApplicationId)
        {
            PropertyApplicationId = propertyApplicationId;
        }
    }
}
