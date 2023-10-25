using MongoDB.Driver;
using Totira.Support.Persistance.Context;

namespace Totira.Support.Persistance.Mongo.Context.Interfaces
{
    public interface IMongoDBContext : IContext
    {
        IMongoCollection<Book> GetCollection<Book>(string name);
    }
}
