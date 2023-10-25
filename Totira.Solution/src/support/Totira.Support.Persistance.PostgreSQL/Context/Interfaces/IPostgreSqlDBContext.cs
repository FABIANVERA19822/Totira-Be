using Microsoft.EntityFrameworkCore;
using Totira.Support.Persistance.Context;

namespace Totira.Support.Persistance.PostgreSQL.Context.Interfaces
{
    public interface IPostgreSqlDBContext : IContext
    {
        DbContext GetDbContext();
    }
}
