// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.AccessAudit;

namespace ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications
{
    public partial interface IReidentificationStorageBroker
    {
        ValueTask<AccessAudit> InsertAccessAuditAsync(AccessAudit accessAudit);
        ValueTask<IQueryable<AccessAudit>> SelectAllAccessAuditAsync();
        ValueTask<AccessAudit> SelectAccessAuditByIdAsync(Guid accessAuditId);
        ValueTask<AccessAudit> UpdateAccessAuditAsync(AccessAudit accessAudit);
        ValueTask<AccessAudit> DeleteAccessAuditAsync(AccessAudit accessAudit);
    }
}
