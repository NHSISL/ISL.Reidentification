// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.Reidentification.Core.Brokers.DateTimes;
using ISL.Reidentification.Core.Brokers.Loggings;
using ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;

namespace ISL.Reidentification.Core.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessService : IDelegatedAccessService
    {
        private readonly IReidentificationStorageBroker reidentificationStorageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DelegatedAccessService(
            IReidentificationStorageBroker reidentificationStorageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.reidentificationStorageBroker = reidentificationStorageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }
        public async ValueTask<DelegatedAccess> AddDelegatedAccessAsync(DelegatedAccess delegatedAccess) =>
            await this.reidentificationStorageBroker.InsertDelegatedAccessAsync(delegatedAccess);
    }
}
