using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;

namespace ISL.ReIdentification.Core.Services.Foundations.Lookups
{
    public partial class LookupService : ILookupService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public LookupService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Lookup> AddLookupAsync(Lookup lookup) =>
            TryCatch(async () =>
            {
                ValidateLookupOnAdd(lookup);

                return await this.storageBroker.InsertLookupAsync(lookup);
            });

        public IQueryable<Lookup> RetrieveAllLookups() =>
            TryCatch(() => this.storageBroker.SelectAllLookups());

        public ValueTask<Lookup> RetrieveLookupByIdAsync(Guid lookupId) =>
            TryCatch(async () =>
            {
                ValidateLookupId(lookupId);

                Lookup maybeLookup = await this.storageBroker
                    .SelectLookupByIdAsync(lookupId);

                ValidateStorageLookup(maybeLookup, lookupId);

                return maybeLookup;
            });
    }
}