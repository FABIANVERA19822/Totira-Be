using Microsoft.EntityFrameworkCore;

namespace Totira.Support.Persistance.PostgreSQL.Context.Interfaces
{
    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();
    }
}
