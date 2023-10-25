using System.Linq.Expressions;

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
        }
    }
}
