// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.Ods
{
    public partial class OdsStorageBroker : EFxceptionsContext, IOdsStorageBroker
    {
        private readonly IConfiguration configuration;

        public OdsStorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            string connectionString = configuration
                .GetConnectionString(name: "OdsConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }

        internal virtual async ValueTask<T> InsertAsync<T>(T @object)
        {
            Entry(@object).State = EntityState.Added;
            await SaveChangesAsync();
            DetachEntity(@object);

            return @object;
        }

        internal virtual async ValueTask<IQueryable<T>> SelectAllAsync<T>() where T : class => Set<T>();

        internal virtual async ValueTask<T> SelectAsync<T>(params object[] @objectIds) where T : class =>
            await FindAsync<T>(@objectIds);

        internal virtual async ValueTask<T> UpdateAsync<T>(T @object)
        {
            Entry(@object).State = EntityState.Modified;
            await SaveChangesAsync();
            DetachEntity(@object);

            return @object;
        }

        internal virtual async ValueTask<T> DeleteAsync<T>(T @object)
        {
            Entry(@object).State = EntityState.Deleted;
            await SaveChangesAsync();
            DetachEntity(@object);

            return @object;
        }

        private void DetachEntity<T>(T @object) =>
            Entry(@object).State = EntityState.Detached;
    }
}