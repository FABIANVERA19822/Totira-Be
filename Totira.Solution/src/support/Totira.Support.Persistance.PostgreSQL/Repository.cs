using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Totira.Support.Persistance.PostgreSQL.Context.Interfaces;
using Totira.Support.Persistance.Util;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Support.Persistance.PostgreSQL
{
    public class Repository<T> : IRepository<T,Guid>
       where T : class, IEntity, IIdentifiable<Guid>

    {
        private readonly DbContext _context;

        public Repository(IPostgreSqlDBContext context)
        {
            _context = context.GetDbContext();
        }
        public async Task Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(typeof(T).Name + " object is null");
            }
            await _context.AddAsync(entity);
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Task DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AsNoTracking().Where(expression).ToListAsync();

        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().AsNoTracking().FirstAsync(e => e.Id == id);
        }

        public Task<IEnumerable<T>> GetManyByIds(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(IMongoFilter<T> filter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetPageAsync(int pageNumber, int pageSize, Expression<Func<T, object>> sortBy, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetPageAsync(IMongoFilter<T> filter, int pageNumber, int pageSize, string propertyName, bool descending)
        {
            throw new NotImplementedException();
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
