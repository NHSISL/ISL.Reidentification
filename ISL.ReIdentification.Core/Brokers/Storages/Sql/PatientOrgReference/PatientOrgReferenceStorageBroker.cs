// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using EFxceptions;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference
{
    public partial class PatientOrgReferenceStorageBroker : EFxceptionsContext, IPatientOrgReferenceStorageBroker
    {
        private readonly IConfiguration configuration;

        public PatientOrgReferenceStorageBroker(IConfiguration configuration)
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
        {
            AddPdsDataConfigurations(modelBuilder.Entity<PdsData>());
        }

        private async ValueTask<T> InsertAsync<T>(T @object)
        {
            Entry(@object).State = EntityState.Added;
            await SaveChangesAsync();
            DetachEntity(@object);

            return @object;
        }

        private async ValueTask<IQueryable<T>> SelectAllAsync<T>() where T : class => Set<T>();

        private async ValueTask<T> SelectAsync<T>(params object[] @objectIds) where T : class =>
            await FindAsync<T>(@objectIds);

        private async ValueTask<T> UpdateAsync<T>(T @object)
        {
            Entry(@object).State = EntityState.Modified;
            await SaveChangesAsync();
            DetachEntity(@object);

            return @object;
        }

        private async ValueTask<T> DeleteAsync<T>(T @object)
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