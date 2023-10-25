using MongoDB.Driver;
using System.Linq.Expressions;
using Totira.Support.Persistance.Mongo.Context.Interfaces;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Support.Persistance.Mongo
{
    public class Repository<T,U> : IRepository<T,U>
        where T : class, IEntity, IIdentifiable<U>

    {

        private readonly IMongoCollection<T> _dbCollection;
        private readonly IMongoDBContext _context;

        public Repository(IMongoDBContext context)
        {
            _context = context;
            _dbCollection = (_context).GetCollection<T>(typeof(T).Name);
        }
        public async Task Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(typeof(T).Name + " object is null");
            }
            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task Delete(T entity)
        {

            await _dbCollection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", entity.Id));
        }

        public async Task DeleteByIdAsync(Guid id)
        {

            await _dbCollection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression)
        {
            var all = await _dbCollection.FindAsync(expression);
            return await all.ToListAsync();
        }

        public async Task<T> GetByIdAsync(U id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);

            return await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
        }
       
  
        public async Task Update(T entity)
        {
            await _dbCollection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", entity.Id), entity);
        }
    }
}
