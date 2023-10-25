using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Totira.Support.Persistance.Mongo;
using Totira.Support.Persistance.Mongo.Context.Interfaces;

namespace Totira.Business.ThirdPartyIntegrationService.Context
{
    public class MongoDBContext : IMongoDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }

        public MongoDBContext(IOptions<MongoSettings> configuration)
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(configuration.Value.Connection);
            settings.DirectConnection = true;


            _mongoClient = new MongoClient(settings);



            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
