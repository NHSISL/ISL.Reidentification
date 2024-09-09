// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;

namespace ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications
{
    public partial interface IReidentificationStorageBroker
    {
        ValueTask<DelegatedAccess> InsertDelegatedAccessAsync(DelegatedAccess delegatedAccess);
        ValueTask<IQueryable<DelegatedAccess>> SelectAllDelegatedAccessesAsync();
        ValueTask<DelegatedAccess> SelectDelegatedAccessByIdAsync(Guid delegateAccessId);
        ValueTask<DelegatedAccess> UpdateDelegatedAccessAsync(DelegatedAccess delegatedAccess);
        ValueTask<DelegatedAccess> DeleteDelegatedAccessAsync(DelegatedAccess delegatedAccess);
    }
}
