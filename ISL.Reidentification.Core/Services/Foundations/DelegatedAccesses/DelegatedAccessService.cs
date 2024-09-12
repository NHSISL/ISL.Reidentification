// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;

namespace ISL.ReIdentification.Core.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessService : IDelegatedAccessService
    {
        private readonly IReIdentificationStorageBroker ReIdentificationStorageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DelegatedAccessService(
            IReIdentificationStorageBroker ReIdentificationStorageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.ReIdentificationStorageBroker = ReIdentificationStorageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }
        public ValueTask<DelegatedAccess> AddDelegatedAccessAsync(DelegatedAccess delegatedAccess) =>
            TryCatch(async () =>
            {
                await ValidateDelegatedAccessOnAdd(delegatedAccess);
                return await this.ReIdentificationStorageBroker.InsertDelegatedAccessAsync(delegatedAccess);
            });
    }
}
