using Microsoft.EntityFrameworkCore;
using Totira.Business.TenantService.Domain;
using Totira.Support.Persistance.PostgreSQL.Context.Interfaces;

namespace Totira.Business.TenantService.Context
{
    public class PostgreSqlDBContext : DbContext, IPostgreSqlDBContext
    {
        public PostgreSqlDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbContext GetDbContext()
        {
            return this;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
        public DbSet<Tenant> Tenant { get; set; }
    }
}
