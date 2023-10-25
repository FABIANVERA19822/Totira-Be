namespace Totira.Support.EventServiceBus.Exceptions
{
    public class MissingRoutingKeyException : Exception
    {
        public MissingRoutingKeyException(Type type) : base($"Class {type.Name} has no RoutingKey attribute")
        {
        }

        public MissingRoutingKeyException(string typeName) : base($"Message type {typeName} has no RoutingKey attribute")
        {
        }
        public MissingRoutingKeyException() : base($"Message has no RoutingKey attribute")
        {
        }
    }
}
