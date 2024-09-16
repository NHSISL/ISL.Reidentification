// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;

namespace ISL.ReIdentification.Core.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessService : IDelegatedAccessService
    {
        private readonly IReIdentificationStorageBroker reIdentificationStorageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DelegatedAccessService(
            IReIdentificationStorageBroker reIdentificationStorageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.reIdentificationStorageBroker = reIdentificationStorageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }
        public ValueTask<DelegatedAccess> AddDelegatedAccessAsync(DelegatedAccess delegatedAccess) =>
            TryCatch(async () =>
            {
                await ValidateDelegatedAccessOnAdd(delegatedAccess);
                return await this.reIdentificationStorageBroker.InsertDelegatedAccessAsync(delegatedAccess);
            });

        public ValueTask<DelegatedAccess> RetrieveDelegatedAccessByIdAsync(Guid delegatedAccessId) =>
            TryCatch(async () =>
            {
                await ValidateDelegatedAccessIdAsync(delegatedAccessId);
                return await this.reIdentificationStorageBroker.SelectDelegatedAccessByIdAsync(delegatedAccessId);
            });

        public ValueTask<IQueryable<DelegatedAccess>> RetrieveAllDelegatedAccessesAsync() =>
            TryCatch(this.reIdentificationStorageBroker.SelectAllDelegatedAccessesAsync);

        public ValueTask<DelegatedAccess> ModifyDelegatedAccessAsync(DelegatedAccess delegatedAccess) =>
            TryCatch(async () =>
            {
                await ValidateDelegatedAccessOnModify(delegatedAccess);

                DelegatedAccess maybeDelegatedAccess =
                    await this.reIdentificationStorageBroker.SelectDelegatedAccessByIdAsync(delegatedAccess.Id);

                await ValidateStorageDelegatedAccessAsync(maybeDelegatedAccess, delegatedAccess.Id);
                await ValidateAgainstStorageDelegatedAccessOnModifyAsync(delegatedAccess, maybeDelegatedAccess);

                return await this.reIdentificationStorageBroker.UpdateDelegatedAccessAsync(delegatedAccess);
            });
    }
}
