// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;

namespace ISL.ReIdentification.Core.Services.Foundations.AccessAudits
{
    public partial class AccessAuditService : IAccessAuditService
    {
        private readonly IReIdentificationStorageBroker reIdentificationStorageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AccessAuditService(
            IReIdentificationStorageBroker reIdentificationStorageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.reIdentificationStorageBroker = reIdentificationStorageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AccessAudit> AddAccessAuditAsync(AccessAudit accessAudit) =>
            TryCatch(async () =>
            {
                await ValidateAccessAuditOnAddAsync(accessAudit);

                return await this.reIdentificationStorageBroker.InsertAccessAuditAsync(accessAudit);
            });

        public ValueTask<IQueryable<AccessAudit>> RetrieveAllAccessAuditsAsync() =>
            TryCatch(this.reIdentificationStorageBroker.SelectAllAccessAuditsAsync);

        public ValueTask<AccessAudit> RetrieveAccessAuditByIdAsync(Guid accessAuditId) =>
            TryCatch(async () =>
            {
                await ValidateAccessAuditOnRetrieveById(accessAuditId);

                var maybeAccessAudit = await this.reIdentificationStorageBroker
                    .SelectAccessAuditByIdAsync(accessAuditId);

                await ValidateStorageAccessAuditAsync(maybeAccessAudit, accessAuditId);

                return maybeAccessAudit;
            });

        public ValueTask<AccessAudit> ModifyAccessAuditAsync(AccessAudit accessAudit) =>
            throw new NotImplementedException();

        public ValueTask<AccessAudit> RemoveAccessAuditByIdAsync(Guid accessAuditId) =>
            throw new NotImplementedException();
    }
}
