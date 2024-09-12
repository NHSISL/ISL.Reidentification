// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using ISL.Reidentification.Core.Models.Foundations.AccessAudit;

namespace ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications
{
    public partial class ReidentificationStorageBroker
    {
       public DbSet<AccessAudit> AccessAudit { get; set; }

        public async ValueTask<AccessAudit> InsertAccessAuditAsync(AccessAudit accessAudit) =>
            await InsertAsync(accessAudit);

        public async ValueTask<IQueryable<AccessAudit>> SelectAllAccessAuditsAsync() =>
            await SelectAllAsync<AccessAudit>();

        public async ValueTask<AccessAudit> SelectAccessAuditByIdAsync(Guid accessAuditId) =>
            await SelectAsync<AccessAudit>(accessAuditId);

        public async ValueTask<AccessAudit> UpdateAccessAuditAsync(AccessAudit accessAudit) =>
            await UpdateAsync(accessAudit);
            
        public async ValueTask<AccessAudit> DeleteAccessAuditAsync(AccessAudit accessAudit) =>
            await DeleteAsync(accessAudit);
    }
}