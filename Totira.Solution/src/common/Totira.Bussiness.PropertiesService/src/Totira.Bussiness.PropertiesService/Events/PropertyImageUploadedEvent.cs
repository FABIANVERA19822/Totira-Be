using Totira.Support.Application.Events;

namespace Totira.Bussiness.PropertiesService.Events;

public class PropertyImageUploadedEvent:IEvent
{
    public string Id { get; set; }
    public PropertyImageUploadedEvent(string id) => Id = id;
}
