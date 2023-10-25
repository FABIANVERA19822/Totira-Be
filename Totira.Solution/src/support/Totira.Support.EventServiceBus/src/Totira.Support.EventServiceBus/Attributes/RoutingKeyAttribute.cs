namespace Totira.Support.EventServiceBus.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RoutingKeyAttribute : Attribute
    {
        public string RoutingKey { get; set; }

        public RoutingKeyAttribute(string routingKey)
        {
            RoutingKey = routingKey;
        }
    }
}
