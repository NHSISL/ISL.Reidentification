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
        public ValueTask<DelegatedAccess> AddDelegatedAccessAsync(DelegatedAccess delegatedAccess) =>
            TryCatch(async () =>
            {
                await ValidateDelegatedAccessOnAdd(delegatedAccess);
                return await this.reidentificationStorageBroker.InsertDelegatedAccessAsync(delegatedAccess);
            });

        public ValueTask<DelegatedAccess> ModifyDelegatedAccessAsync(DelegatedAccess delegatedAccess) =>
            TryCatch(async () =>
            {
                await ValidateDelegatedAccessOnModify(delegatedAccess);

                DelegatedAccess maybeDelegatedAccess =
                    await this.reidentificationStorageBroker.SelectDelegatedAccessByIdAsync(delegatedAccess.Id);

                await ValidateStorageDelegatedAccessAsync(maybeDelegatedAccess, delegatedAccess.Id);
                await ValidateAgainstStorageDelegatedAccessOnModifyAsync(delegatedAccess, maybeDelegatedAccess);

                return await this.reidentificationStorageBroker.UpdateDelegatedAccessAsync(delegatedAccess);
            });
    }
}
