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

        public ValueTask<DelegatedAccess> ModifyDelegatedAccessAsync(DelegatedAccess delegatedAccess) =>
            TryCatch(async () =>
            {
                await ValidateDelegatedAccessOnModify(delegatedAccess);

                DelegatedAccess maybeDelegatedAccess =
                    await this.ReIdentificationStorageBroker.SelectDelegatedAccessByIdAsync(delegatedAccess.Id);

                await ValidateStorageDelegatedAccessAsync(maybeDelegatedAccess, delegatedAccess.Id);
                await ValidateAgainstStorageDelegatedAccessOnModifyAsync(delegatedAccess, maybeDelegatedAccess);

                return await this.ReIdentificationStorageBroker.UpdateDelegatedAccessAsync(delegatedAccess);
            });
    }
}
