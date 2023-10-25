using MongoDB.Driver;
using System.Linq.Expressions;
using Totira.Support.Persistance.Mongo.Context.Interfaces;
using Totira.Support.Persistance.Mongo.Util;
using Totira.Support.Persistance.Util;
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

        public async Task<IEnumerable<T>> GetManyByIds(IEnumerable<U> ids)
        {
            var filter = Builders<T>.Filter.In("_id", ids);
            var result = await _dbCollection
                .Find(filter)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> GetPageAsync(int pageNumber, int pageSize, Expression<Func<T, object>> sortBy, bool descending = false)
        {
            int skip = (pageNumber - 1) * pageSize;
            var sort = descending
                ? Builders<T>.Sort.Descending(sortBy)
                : Builders<T>.Sort.Ascending(sortBy);

            return await _dbCollection
                    .Find("{}")
                    .Sort(sort)
                    .Skip(skip)
                    .Limit(pageSize)
                    .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPageAsync(IMongoFilter<T> filter, int pageNumber, int pageSize, string propertyName, bool descending)
        {
            if (filter is not MongoFilter<T> filterObject)
            {
                throw new ArgumentException("Filter must be of type MongoFilter<T>.", nameof(filter));
            }

            int skip = (pageNumber - 1) * pageSize;
            var sort = descending
                ? Builders<T>.Sort.Descending(propertyName)
                : Builders<T>.Sort.Ascending(propertyName);

            return await _dbCollection
                    .Find(filterObject.GetFilterDefinition())
                    .Sort(sort)
                    .Skip(skip)
                    .Limit(pageSize)
                    .ToListAsync();
        }

        public async Task<long> GetCountAsync()
        {
            return await _dbCollection.CountDocumentsAsync(_ => true);
        }

        public async Task<long> GetCountAsync(IMongoFilter<T> filter)
        {
            if (filter is not MongoFilter<T> filterObject)
            {
                throw new ArgumentException("Filter must be of type MongoFilter<T>.", nameof(filter));
            }

            return await _dbCollection.CountDocumentsAsync(filterObject.GetFilterDefinition());
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
