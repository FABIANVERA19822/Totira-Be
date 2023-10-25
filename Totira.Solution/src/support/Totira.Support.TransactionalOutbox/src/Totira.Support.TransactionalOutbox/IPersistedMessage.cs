using Totira.Support.Persistance;

namespace Totira.Support.TransactionalOutbox
{

    public interface IPersistedMessage : IIdentifiable<Guid>
    {
        string SerializedBody { get; }

        string RoutingKey { get; }

        DateTimeOffset CreatedOn { get; }
    }
}
