using System.Diagnostics.CodeAnalysis;
using Totira.Support.Persistance.Entities;

namespace Totira.Support.TransactionalOutbox
{
    public class MessageOutbox : Entity, IPersistedMessage, IEquatable<MessageOutbox>
    {
        public string SerializedBody { get; protected set; }

        public string RoutingKey { get; protected set; }

        public DateTimeOffset CreatedOn { get; protected set; }

        public MessageOutbox(Guid id, string serializedBody, string routingKey, DateTimeOffset createdOn)
        {
            Id = id;
            SerializedBody = serializedBody;
            RoutingKey = routingKey;
            CreatedOn = createdOn;
        }

        public bool Equals([AllowNull] MessageOutbox other)
        {
            if (other == null)
                return false;

            return Id == other.Id
                && SerializedBody == other.SerializedBody
                && RoutingKey == other.RoutingKey;
        }
    }
}
