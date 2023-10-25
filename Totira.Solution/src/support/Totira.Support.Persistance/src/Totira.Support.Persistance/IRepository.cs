using System.Linq.Expressions;
using Totira.Support.Persistance.Util;

namespace Totira.Support.Persistance
{
    public interface IRepository
    {
        public interface IRepository<T,U> where T : class, IEntity, IIdentifiable<U>
        {

            Task<T> GetByIdAsync(U id);

            Task Add(T entity);

            Task Update(T entity);

            Task Delete(T entity);
            Task DeleteByIdAsync(Guid id);

            Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression);

            Task<IEnumerable<T>> GetPageAsync(int pageNumber, int pageSize, Expression<Func<T, object>> sortBy, bool descending = false);
            Task<IEnumerable<T>> GetPageAsync(IMongoFilter<T> filter, int pageNumber, int pageSize, string propertyName, bool descending);
            Task<IEnumerable<T>> GetManyByIds(IEnumerable<U> ids);
            Task<long> GetCountAsync();
            Task<long> GetCountAsync(IMongoFilter<T> filter);
        }
    }
}
