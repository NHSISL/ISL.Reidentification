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

        public async ValueTask<AccessAudit> InsertAccessAuditsAsync(AccessAudit accessAudit) =>
            await InsertAsync(accessAudit);

        public async ValueTask<IQueryable<AccessAudit>> SelectAllAccessAuditsesAsync() =>
            await SelectAllAsync<AccessAudit>();

        public async ValueTask<AccessAudit> SelectAccessAuditsByIdAsync(Guid accessAuditId) =>
            await SelectAsync<AccessAudit>(accessAuditId);

        public async ValueTask<AccessAudit> UpdateAccessAuditsAsync(AccessAudit accessAudit) =>
            await UpdateAsync(accessAudit);
            
        public async ValueTask<AccessAudit> DeleteAccessAuditsAsync(AccessAudit accessAudit) =>
            await DeleteAsync(accessAudit);
    }
}