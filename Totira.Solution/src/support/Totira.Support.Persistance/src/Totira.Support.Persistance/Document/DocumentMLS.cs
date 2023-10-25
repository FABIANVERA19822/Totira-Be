using MongoDB.Bson.Serialization.Attributes;

namespace Totira.Support.Persistance.Document
{
    public class DocumentMLS : IEntity, IIdentifiable<string>
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;

    }
}
