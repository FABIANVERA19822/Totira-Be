using MongoDB.Bson.Serialization.Attributes;

namespace Totira.Support.Persistance.Document
{
    public class Document : IEntity, IIdentifiable<Guid>
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}
